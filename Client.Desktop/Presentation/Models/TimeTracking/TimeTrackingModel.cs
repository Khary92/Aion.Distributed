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
using Client.Desktop.DataModels.Local;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;
using ListEx = DynamicData.ListEx;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeTrackingModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITimeSlotViewModelFactory timeSlotViewModelFactory,
    ITraceCollector tracer) : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private int _currentViewModelIndex;
    private ObservableCollection<TicketClientModel> _filteredTickets = [];
    private TicketClientModel? _selectedTicket;
    private string _selectedTicketName = string.Empty;

    private ObservableCollection<TimeSlotViewModel> _timeSlotViewModels = [];
    private SettingsClientModel? _localSettings;
    
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
        var currentSprint = await requestSender.Send(new ClientGetActiveSprintRequest(Guid.NewGuid()));

        if (currentSprint == null) return;

        FilteredTickets.Clear();
        var tickets = await requestSender.Send(new ClientGetTicketsForCurrentSprintRequest(Guid.NewGuid()));
        ListEx.AddRange(FilteredTickets, tickets);
    }

    private async Task LoadTimeSlotViewModels()
    {
        TimeSlotViewModels.Clear();

        var controlDataList =
            await requestSender.Send(
                new ClientGetTimeSlotControlDataRequest(_localSettings!.SelectedDate, Guid.NewGuid()));

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

    public void RegisterMessenger()
    {
        messenger.Register<SettingsClientModel>(this, async void (_, m) =>
        {
            _localSettings = m;
            await InitializeAsync();
            await LoadTimeSlotViewModels();
        });
        
        messenger.Register<WorkDaySelectedNotification>(this, async void (_, m) =>
        {
            _localSettings!.SelectedDate = m.Date;
            await InitializeAsync();
        });
        
        messenger.Register<NewTicketMessage>(this, async void (_, message) =>
        {
            AllTickets.Add(message.Ticket);
            await tracer.Ticket.Create.AggregateAdded(GetType(), message.TraceId);

            var currentSprint = await requestSender.Send(new ClientGetActiveSprintRequest(Guid.NewGuid()));

            if (currentSprint == null) return;

            if (currentSprint.TicketIds.Contains(message.Ticket.TicketId))
                FilteredTickets.Add(message.Ticket);
        });

        messenger.Register<ClientTicketDataUpdatedNotification>(this, async void (_, notification) =>
        {
            var ticketClientModel = AllTickets.FirstOrDefault(tsv => tsv.TicketId == notification.TicketId);

            if (ticketClientModel == null)
            {
                await tracer.Ticket.Update.NoAggregateFound(GetType(), notification.TraceId);
                return;
            }

            ticketClientModel.Apply(notification);
            await tracer.Ticket.Update.ChangesApplied(GetType(), notification.TraceId);
        });

        //TODO This implementation is bad. Fix that 
        messenger.Register<ClientTicketAddedToSprintNotification>(this,
            async void (_, _) => { await InitializeAsync(); });
        messenger.Register<ClientTicketAddedToActiveSprintNotification>(this,
            async void (_, _) => { await InitializeAsync(); });

        messenger.Register<ClientTimeSlotControlCreatedNotification>(this, async void (_, notification) =>
        {
            TimeSlotViewModels.Add(await
                timeSlotViewModelFactory.Create(notification.Ticket, notification.StatisticsData,
                    notification.TimeSlot));

            CurrentViewModelIndex = TimeSlotViewModels.Count - 1;
            SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
        });

        messenger.Register<ClientSprintSelectionChangedNotification>(this, async void (_, _) =>
        {
            FilteredTickets.Clear();

            var currentSprint = await requestSender.Send(new ClientGetActiveSprintRequest(Guid.NewGuid()));

            if (currentSprint == null)
            {
                throw new InvalidOperationException("No active sprint");
            }

            var ticketClientModels = await requestSender.Send(new ClientGetAllTicketsRequest(Guid.NewGuid()));
            foreach (var modelTicket in ticketClientModels.Where(modelTicket =>
                         modelTicket.SprintIds.Contains(currentSprint.SprintId)))
                FilteredTickets.Add(modelTicket);
        });

        messenger.Register<ClientWorkDaySelectionChangedNotification>(this, async void (_, _) =>
        {
            FilteredTickets.Clear();

            var currentSprint = await requestSender.Send(new ClientGetActiveSprintRequest(Guid.NewGuid()));

            if (currentSprint == null) throw new InvalidOperationException("No active sprint");

            var ticketDtos = await requestSender.Send(new ClientGetAllTicketsRequest(Guid.NewGuid()));

            foreach (var ticket in ticketDtos.Where(modelTicket =>
                         modelTicket.SprintIds.Contains(currentSprint.SprintId)))
                FilteredTickets.Add(ticket);

            TimeSlotViewModels.Clear();
            await LoadTimeSlotViewModels();
        });
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
            _localSettings!.SelectedDate, Guid.NewGuid()));
    }
}