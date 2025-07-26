using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Notifications.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Services.Initializer;
using Client.Desktop.Services.LocalSettings;
using CommunityToolkit.Mvvm.Messaging;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.UseCases;
using Proto.Notifications.Sprint;
using Proto.Notifications.Ticket;
using Proto.Notifications.UseCase;
using Proto.Requests.Sprints;
using Proto.Requests.Tickets;
using Proto.Requests.UseCase;
using ReactiveUI;
using ListEx = DynamicData.ListEx;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeTrackingModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    ILocalSettingsService localSettingsService,
    IMessenger messenger,
    ITimeSlotViewModelFactory timeSlotViewModelFactory) : ReactiveObject, IInitializeAsync, IRegisterMessenger
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
        var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

        if (currentSprint == null) return;

        FilteredTickets.Clear();
        var tickets = await requestSender.Send(new GetTicketsForCurrentSprintRequestProto());
        ListEx.AddRange(FilteredTickets, tickets);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTicketMessage>(this, async void (_, m) =>
        {
            AllTickets.Add(m.Ticket);

            var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

            if (currentSprint == null) return;

            if (currentSprint.TicketIds.Contains(m.Ticket.TicketId))
                FilteredTickets.Add(m.Ticket);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            var ticketDto = AllTickets.FirstOrDefault(tsv => tsv.TicketId == Guid.Parse(m.TicketId));

            if (ticketDto == null) return;

            ticketDto.Apply(m);
        });

        //TODO This implementation is bad. Fix that 
        messenger.Register<TicketAddedToSprintNotification>(this, async void (_, _) => { await InitializeAsync(); });
        messenger.Register<TicketAddedToActiveSprintNotification>(this,
            async void (_, _) => { await InitializeAsync(); });

        messenger.Register<TimeSlotControlCreatedNotification>(this, async void (_, m) =>
        {
            TimeSlotViewModels.Add(await
                timeSlotViewModelFactory.Create(m.TimeSlotControlData.TicketProto.ToModel(),
                    m.TimeSlotControlData.StatisticsDataProto.ToModel(),
                    m.TimeSlotControlData.TimeSlotProto.ToModel()));

            CurrentViewModelIndex = TimeSlotViewModels.Count - 1;
            SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
        });

        messenger.Register<SprintSelectionChangedNotification>(this, async void (_, _) =>
        {
            FilteredTickets.Clear();

            var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

            if (currentSprint == null) throw new InvalidOperationException("No active sprint");

            var ticketDtos = await requestSender.Send(new GetAllTicketsRequestProto());
            foreach (var modelTicket in ticketDtos.Where(modelTicket =>
                         modelTicket.SprintIds.Contains(currentSprint.SprintId)))
                FilteredTickets.Add(modelTicket);
        });

        messenger.Register<WorkDaySelectionChangedNotification>(this, async void (_, _) =>
        {
            FilteredTickets.Clear();

            var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

            if (currentSprint == null) throw new InvalidOperationException("No active sprint");

            var ticketDtos = await requestSender.Send(new GetAllTicketsRequestProto());

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

    public async Task LoadTimeSlotViewModels()
    {
        TimeSlotViewModels.Clear();

        var controlDataList = await requestSender.Send(new GetTimeSlotControlDataRequestProto
        {
            Date = Timestamp.FromDateTimeOffset(localSettingsService.SelectedDate)
        });

        foreach (var controlData in controlDataList.TimeSlotControlData)
        {
            var timeSlotViewModel = await timeSlotViewModelFactory.Create(controlData.TicketProto.ToModel(),
                controlData.StatisticsDataProto.ToModel(), controlData.TimeSlotProto.ToModel());
            TimeSlotViewModels.Add(timeSlotViewModel);
        }

        if (TimeSlotViewModels.Any())
        {
            CurrentViewModelIndex = TimeSlotViewModels.Count - 1;
            SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
        }
    }

    public async Task CreateNewTimeSlotViewModel()
    {
        if (SelectedTicket == null) return;

        await commandSender.Send(new CreateTimeSlotControlCommandProto
        {
            TicketId = SelectedTicket.TicketId.ToString()
        });
    }
}