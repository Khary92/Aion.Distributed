using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Client.Records;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeTrackingModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    ITrackingSlotViewModelFactory trackingSlotViewModelFactory,
    ILocalSettingsService localSettingsService,
    ITraceCollector tracer,
    INotificationPublisherFacade notificationFacade) : ReactiveObject, IInitializeAsync, IEventRegistration

{
    private int _currentViewModelIndex;
    private ObservableCollection<TicketClientModel> _filteredTickets = [];
    private TicketClientModel? _selectedTicket;
    private string _selectedTicketName = string.Empty;

    private ObservableCollection<TrackingSlotViewModel> _trackingSlotViewModels = [];

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

    public ObservableCollection<TrackingSlotViewModel> TrackingSlotViewModels
    {
        get => _trackingSlotViewModels;
        set => this.RaiseAndSetIfChanged(ref _trackingSlotViewModels, value);
    }

    private ObservableCollection<TicketClientModel> AllTickets { get; } = [];

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        var ticketsInSprint = await requestSender.Send(new ClientGetTicketsForCurrentSprintRequest());
        var allTickets = await requestSender.Send(new ClientGetAllTicketsRequest());

        FilteredTickets.Clear();
        FilteredTickets.AddRange(ticketsInSprint);

        AllTickets.Clear();
        AllTickets.AddRange(allTickets);

        await LoadTimeSlotViewModels();
    }

    public void RegisterMessenger()
    {
        notificationFacade.Ticket.NewTicketNotificationReceived += HandleNewTicketMessage;
        notificationFacade.Ticket.TicketDataUpdatedNotificationReceived += HandleTicketDataUpdatedNotification;
        notificationFacade.Sprint.ClientTicketAddedToActiveSprintNotificationReceived +=
            HandleTicketAddedToActiveSprintNotification;
        notificationFacade.Client.ClientSprintSelectionChangedNotificationReceived +=
            HandleTicketSprintSelectionChangedNotification;
        notificationFacade.Client.ClientTrackingControlCreatedNotificationReceived +=
            HandleTimeSlotControlCreatedNotification;
        //TODO IRecipient<ClientWorkDaySelectionChangedNotification>
    }

    public void UnregisterMessenger()
    {
        notificationFacade.Ticket.NewTicketNotificationReceived -= HandleNewTicketMessage;
        notificationFacade.Ticket.TicketDataUpdatedNotificationReceived -= HandleTicketDataUpdatedNotification;
        notificationFacade.Sprint.ClientTicketAddedToActiveSprintNotificationReceived -=
            HandleTicketAddedToActiveSprintNotification;
        notificationFacade.Client.ClientSprintSelectionChangedNotificationReceived -=
            HandleTicketSprintSelectionChangedNotification;
        notificationFacade.Client.ClientTrackingControlCreatedNotificationReceived -=
            HandleTimeSlotControlCreatedNotification;
    }

    private async Task LoadTimeSlotViewModels()
    {
        var controlDataList =
            await requestSender.Send(
                new ClientGetTrackingControlDataRequest(localSettingsService.SelectedDate, Guid.NewGuid()));

        var trackingSlotViewModels = new List<TrackingSlotViewModel>();
        foreach (var controlData in controlDataList)
        {
            var timeSlotViewModel = await trackingSlotViewModelFactory.Create(
                controlData.Ticket,
                controlData.StatisticsData,
                controlData.TimeSlot);
            trackingSlotViewModels.Add(timeSlotViewModel);
        }

        TrackingSlotViewModels.Clear();
        foreach (var vm in trackingSlotViewModels)
            TrackingSlotViewModels.Add(vm);

        if (TrackingSlotViewModels.Any())
        {
            CurrentViewModelIndex = TrackingSlotViewModels.Count - 1;
            SelectedTicketName = TrackingSlotViewModels[CurrentViewModelIndex].Model.Ticket.Name;
        }
    }

    public void TogglePreviousViewModel()
    {
        if (CurrentViewModelIndex <= 0) return;

        ResetTrackingSlotState();
        CurrentViewModelIndex -= 1;
        SelectedTicketName = TrackingSlotViewModels[CurrentViewModelIndex].Model.Ticket.Name;
    }

    public void ToggleNextViewModel()
    {
        if (CurrentViewModelIndex == TrackingSlotViewModels.Count - 1) return;

        ResetTrackingSlotState();
        CurrentViewModelIndex += 1;
        SelectedTicketName = TrackingSlotViewModels[CurrentViewModelIndex].Model.Ticket.Name;
    }

    private void ResetTrackingSlotState()
    {
        TrackingSlotViewModels[CurrentViewModelIndex].Model.TimeSlot.IsTimerRunning = false;
        TrackingSlotViewModels[CurrentViewModelIndex].Model.Ticket.IsReplayMode = false;
    }

    public async Task CreateNewTimeSlotViewModel()
    {
        if (SelectedTicket == null) return;

        var traceId = Guid.NewGuid();
        await tracer.Client.CreateTrackingControl.StartUseCase(GetType(), traceId);

        var clientCreateTrackingControlCommand = new ClientCreateTrackingControlCommand(SelectedTicket.TicketId,
            localSettingsService.SelectedDate, traceId);

        await tracer.Client.CreateTrackingControl.SendingCommand(GetType(), traceId,
            clientCreateTrackingControlCommand);
        await commandSender.Send(clientCreateTrackingControlCommand);
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

    private async Task HandleTicketAddedToActiveSprintNotification(ClientTicketAddedToActiveSprintNotification message)
    {
        await InitializeAsync();
    }

    private async Task HandleTicketDataUpdatedNotification(ClientTicketDataUpdatedNotification message)
    {
        var ticketClientModel = AllTickets.FirstOrDefault(tsv => tsv.TicketId == message.TicketId);

        if (ticketClientModel == null)
        {
            await tracer.Ticket.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        ticketClientModel.Apply(message);

        await tracer.Ticket.Update.ChangesApplied(GetType(), message.TraceId);
    }

    private async Task HandleTicketSprintSelectionChangedNotification(ClientSprintSelectionChangedNotification message)
    {
        var ticketClientModels = await requestSender.Send(new ClientGetTicketsForCurrentSprintRequest());


        FilteredTickets.Clear();
        FilteredTickets.AddRange(ticketClientModels);
    }

    private async Task HandleTimeSlotControlCreatedNotification(ClientTrackingControlCreatedNotification message)
    {
        var trackingSlotViewModel =
            await trackingSlotViewModelFactory.Create(message.Ticket, message.StatisticsData, message.TimeSlot);


        TrackingSlotViewModels.Add(trackingSlotViewModel);
        CurrentViewModelIndex = TrackingSlotViewModels.Count - 1;
        SelectedTicketName = TrackingSlotViewModels[CurrentViewModelIndex].Model.Ticket.Name;


        await tracer.Client.CreateTrackingControl.AggregateAdded(GetType(), message.TraceId);
    }

    private async Task HandleWorkDaySelectionChangedNotification(ClientWorkDaySelectionChangedNotification message)
    {
        var ticketModels = await requestSender.Send(new ClientGetAllTicketsRequest());


        FilteredTickets.Clear();
        FilteredTickets.AddRange(ticketModels);
        TrackingSlotViewModels.Clear();


        await LoadTimeSlotViewModels();
    }
}