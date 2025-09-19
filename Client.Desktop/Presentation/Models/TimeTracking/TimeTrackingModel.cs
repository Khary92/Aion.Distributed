using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.UseCases.Records;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.Communication.Requests.UseCase.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeTrackingModel(
    IMessenger messenger,
    ICommandSender commandSender,
    IRequestSender requestSender,
    ITimeSlotViewModelFactory timeSlotViewModelFactory,
    ILocalSettingsService localSettingsService,
    ITraceCollector tracer) : ReactiveObject, IInitializeAsync, IMessengerRegistration, IRecipient<NewTicketMessage>,
    IRecipient<ClientTicketDataUpdatedNotification>, IRecipient<ClientTicketAddedToActiveSprintNotification>,
    IRecipient<ClientSprintSelectionChangedNotification>, IRecipient<ClientTimeSlotControlCreatedNotification>,
    IRecipient<ClientWorkDaySelectionChangedNotification>
{
    private int _currentViewModelIndex;
    private ObservableCollection<TicketClientModel> _filteredTickets = [];
    private TicketClientModel? _selectedTicket;
    private string _selectedTicketName = string.Empty;

    private ObservableCollection<TimeSlotViewModel> _timeSlotViewModels = [];

    public string SelectedTicketName
    {
        get => _selectedTicketName;
        set => this.RaiseAndSetIfChanged(ref _selectedTicketName, value);
    }

    public TicketClientModel? SelectedTicket
    {
        get => _selectedTicket;
        set => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
    }

    public int CurrentViewModelIndex
    {
        get => _currentViewModelIndex;
        set => this.RaiseAndSetIfChanged(ref _currentViewModelIndex, value);
    }

    public ObservableCollection<TicketClientModel> FilteredTickets
    {
        get => _filteredTickets;
        set => this.RaiseAndSetIfChanged(ref _filteredTickets, value);
    }

    public ObservableCollection<TimeSlotViewModel> TimeSlotViewModels
    {
        get => _timeSlotViewModels;
        set => this.RaiseAndSetIfChanged(ref _timeSlotViewModels, value);
    }

    private ObservableCollection<TicketClientModel> AllTickets { get; } = [];

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        FilteredTickets.Clear();
        var ticketsInSprint = await requestSender.Send(new ClientGetTicketsForCurrentSprintRequest());
        FilteredTickets.AddRange(ticketsInSprint);

        AllTickets.Clear();
        var allTickets = await requestSender.Send(new ClientGetAllTicketsRequest());
        AllTickets.AddRange(allTickets);

        await LoadTimeSlotViewModels();
    }

    public void RegisterMessenger()
    {
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Receive(ClientSprintSelectionChangedNotification message)
    {
        _ = HandleTicketSprintSelectionChangedNotification(message);
    }

    //TODO Fix this!
    public void Receive(ClientTicketAddedToActiveSprintNotification message)
    {
        _ = InitializeAsync();
    }

    public void Receive(ClientTicketDataUpdatedNotification message)
    {
        var ticketClientModel = AllTickets.FirstOrDefault(tsv => tsv.TicketId == message.TicketId);

        if (ticketClientModel == null)
        {
            _ = tracer.Ticket.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        ticketClientModel.Apply(message);
        _ = tracer.Ticket.Update.ChangesApplied(GetType(), message.TraceId);
    }

    public void Receive(ClientTimeSlotControlCreatedNotification message)
    {
        _ = HandleTimeSlotControlCreatedNotification(message);
    }

    public void Receive(ClientWorkDaySelectionChangedNotification message)
    {
        _ = HandleWorkDaySelectionChangedNotification(message);
    }

    public void Receive(NewTicketMessage message)
    {
        _ = HandleNewTicketMessage(message);
    }

    private async Task LoadTimeSlotViewModels()
    {
        TimeSlotViewModels.Clear();

        var controlDataList =
            await requestSender.Send(
                new ClientGetTimeSlotControlDataRequest(localSettingsService.SelectedDate, Guid.NewGuid()));

        foreach (var controlData in controlDataList)
        {
            var timeSlotViewModel = await timeSlotViewModelFactory.Create(controlData.Ticket,
                controlData.StatisticsData, controlData.TimeSlot);
            TimeSlotViewModels.Add(timeSlotViewModel);
        }

        if (TimeSlotViewModels.Any())
        {
            CurrentViewModelIndex = TimeSlotViewModels.Count - 1;
            SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
        }
    }

    public void TogglePreviousViewModel()
    {
        if (CurrentViewModelIndex <= 0) return;

        TimeSlotViewModels[CurrentViewModelIndex].ToggleTimerCommand.Execute();
        CurrentViewModelIndex -= 1;
        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }

    public void ToggleNextViewModel()
    {
        if (CurrentViewModelIndex == TimeSlotViewModels.Count - 1) return;

        TimeSlotViewModels[CurrentViewModelIndex].ToggleTimerCommand.Execute();
        CurrentViewModelIndex += 1;
        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }

    public async Task CreateNewTimeSlotViewModel()
    {
        if (SelectedTicket == null) return;

        await commandSender.Send(new ClientCreateTimeSlotControlCommand(SelectedTicket.TicketId,
            localSettingsService.SelectedDate, Guid.NewGuid()));
    }

    private async Task HandleNewTicketMessage(NewTicketMessage message)
    {
        AllTickets.Add(message.Ticket);
        await tracer.Ticket.Create.AggregateAdded(GetType(), message.TraceId);

        var currentSprint = await requestSender.Send(new ClientGetActiveSprintRequest());

        if (currentSprint == null) return;

        if (currentSprint.TicketIds.Contains(message.Ticket.TicketId))
            FilteredTickets.Add(message.Ticket);
    }

    private async Task HandleTicketSprintSelectionChangedNotification(ClientSprintSelectionChangedNotification message)
    {
        FilteredTickets.Clear();
        var ticketClientModels = await requestSender.Send(new ClientGetTicketsForCurrentSprintRequest());
        FilteredTickets.Add(ticketClientModels);
    }

    private async Task HandleTimeSlotControlCreatedNotification(ClientTimeSlotControlCreatedNotification message)
    {
        TimeSlotViewModels.Add(await
            timeSlotViewModelFactory.Create(message.Ticket, message.StatisticsData,
                message.TimeSlot));

        CurrentViewModelIndex = TimeSlotViewModels.Count - 1;
        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }

    private async Task HandleWorkDaySelectionChangedNotification(ClientWorkDaySelectionChangedNotification message)
    {
        FilteredTickets.Clear();
        
        var ticketModels = await requestSender.Send(new ClientGetAllTicketsRequest());
        
        FilteredTickets.Add(ticketModels);

        TimeSlotViewModels.Clear();
        await LoadTimeSlotViewModels();
    }
}