using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Factories;
using Client.Desktop.Views.Custom;
using Contract.CQRS.Requests.Replays;
using Contract.CQRS.Requests.Sprints;
using Contract.CQRS.Requests.StatisticsData;
using Contract.CQRS.Requests.Tickets;
using Contract.CQRS.Requests.TimeSlots;
using Contract.CQRS.Requests.WorkDays;
using Contract.DTO;
using MediatR;
using Proto.Command.UseCases;
using Proto.Notifications.UseCase;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.TimeTracking;

//TODO reimplement
public class TimeTrackingViewModel : ReactiveObject
// INotificationHandler<SprintSelectionChangedNotification>,
// INotificationHandler<TimeSlotControlCreatedNotification>,
// INotificationHandler<WorkDaySelectionChangedNotification>
{
    private readonly IMediator _mediator;

    private readonly ITimeSlotViewModelFactory _timeSlotViewModelFactory;

    private int _currentViewModelIndex;
    private TicketDto? _selectedTicket;

    private string _selectedTicketName = string.Empty;

    private ObservableCollection<TimeSlotViewModel> _timeSlotViewModels = [];

    public TimeTrackingViewModel(IMediator mediator, ITimeSlotViewModelFactory timeSlotViewModelFactory,
        TimeTrackingModel timeTrackingModel, INoteViewFactory noteViewFactory)
    {
        _mediator = mediator;
        _timeSlotViewModelFactory = timeSlotViewModelFactory;

        Model = timeTrackingModel;
        AddTimeSlotControlCommand =
            ReactiveCommand.CreateFromTask(CreateNewTimeSlotViewModel);

        NextViewModelCommand = ReactiveCommand.Create(ToggleNextViewModel);
        PreviousViewModelCommand = ReactiveCommand.Create(TogglePreviousViewModel);

        Initialize().ConfigureAwait(false);
    }

    public string SelectedTicketName
    {
        get => _selectedTicketName;
        set => this.RaiseAndSetIfChanged(ref _selectedTicketName, value);
    }

    public TimeTrackingModel Model { get; }

    public ObservableCollection<TimeSlotViewModel> TimeSlotViewModels
    {
        get => _timeSlotViewModels;
        set => this.RaiseAndSetIfChanged(ref _timeSlotViewModels, value);
    }

    public TicketDto? SelectedTicket
    {
        get => _selectedTicket;
        set => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
    }

    public int CurrentViewModelIndex
    {
        get => _currentViewModelIndex;
        set => this.RaiseAndSetIfChanged(ref _currentViewModelIndex, value);
    }

    public ReactiveCommand<Unit, Unit> AddTimeSlotControlCommand { get; }
    public ReactiveCommand<Unit, Unit> PreviousViewModelCommand { get; }
    public ReactiveCommand<Unit, Unit> NextViewModelCommand { get; }

    public async Task Handle(SprintSelectionChangedNotification notification, CancellationToken cancellationToken)
    {
        Model.FilteredTickets.Clear();

        var currentSprint = await _mediator.Send(new GetActiveSprintRequest(), cancellationToken);

        if (currentSprint == null) throw new InvalidOperationException("No active sprint");

        var ticketDtos = await _mediator.Send(new GetAllTicketsRequest(), cancellationToken);
        foreach (var modelTicket in ticketDtos.Where(modelTicket =>
                     modelTicket.SprintIds.Contains(currentSprint.SprintId)))
            Model.FilteredTickets.Add(modelTicket);
    }

    public async Task Handle(TimeSlotControlCreatedNotification notification, CancellationToken cancellationToken)
    {
        var timeSlotViewModel = TimeSlotViewModels.FirstOrDefault(tsv => tsv.ViewId == Guid.Parse(notification.ViewId));

        if (timeSlotViewModel == null) return;

        timeSlotViewModel.Model.TimeSlot =
            await _mediator.Send(new GetTimeSlotByIdRequest(Guid.Parse(notification.TimeSlotId)), cancellationToken);

        var ticketReplayDecorator =
            await _mediator.Send(new GetTicketReplayByIdRequest(Guid.Parse(notification.TicketId)), cancellationToken);
        SelectedTicketName = ticketReplayDecorator.Ticket.Name;
        timeSlotViewModel.Model.TicketReplayDecorator = ticketReplayDecorator;

        timeSlotViewModel.StatisticsViewModel.StatisticsData =
            await _mediator.Send(new GetStatisticsDataByTimeSlotIdRequest(Guid.Parse(notification.TimeSlotId)),
                cancellationToken);
        await timeSlotViewModel.StatisticsViewModel.Initialize();
        timeSlotViewModel.StatisticsViewModel.RegisterMessenger();

        timeSlotViewModel.NoteStreamViewModel.TimeSlotId = Guid.Parse(notification.TimeSlotId);
        await timeSlotViewModel.NoteStreamViewModel.InitializeAsync();
        timeSlotViewModel.NoteStreamViewModel.RegisterMessenger();

        timeSlotViewModel.InitializeViewTimer(new ViewTimer());

        CurrentViewModelIndex = TimeSlotViewModels.IndexOf(timeSlotViewModel);
    }

    public async Task Handle(WorkDaySelectionChangedNotification notification, CancellationToken cancellationToken)
    {
        Model.FilteredTickets.Clear();

        var currentSprint = await _mediator.Send(new GetActiveSprintRequest(), cancellationToken);

        if (currentSprint == null) throw new InvalidOperationException("No active sprint");

        var ticketDtos = await _mediator.Send(new GetAllTicketsRequest(), cancellationToken);

        foreach (var ticket in ticketDtos.Where(modelTicket => modelTicket.SprintIds.Contains(currentSprint.SprintId)))
            Model.FilteredTickets.Add(ticket);

        TimeSlotViewModels.Clear();
        await LoadTimeSlotViewModels();
    }

    private void TogglePreviousViewModel()
    {
        if (CurrentViewModelIndex <= 0) return;

        TimeSlotViewModels[CurrentViewModelIndex].ToggleTimerCommand.Execute();
        CurrentViewModelIndex -= 1;
        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }

    private void ToggleNextViewModel()
    {
        if (CurrentViewModelIndex == TimeSlotViewModels.Count - 1) return;

        TimeSlotViewModels[CurrentViewModelIndex].ToggleTimerCommand.Execute();
        CurrentViewModelIndex += 1;
        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }

    private async Task Initialize()
    {
        var currentSprint = await _mediator.Send(new GetActiveSprintRequest());

        if (currentSprint == null) throw new InvalidOperationException("No active sprint");

        await Model.Initialize();
        Model.RegisterMessenger();

        if (Model.FilteredTickets.Count != 0) SelectedTicket = Model.FilteredTickets[0];
    }

    public async Task LoadTimeSlotViewModels()
    {
        TimeSlotViewModels.Clear();

        var selectedWorkDay = await _mediator.Send(new GetSelectedWorkDayRequest());
        var timeSlots = await _mediator.Send(new GetTimeSlotsForWorkDayIdRequest(selectedWorkDay.WorkDayId));

        foreach (var timeSlotDto in timeSlots)
        {
            var timeSlotViewModel = _timeSlotViewModelFactory.Create();
            TimeSlotViewModels.Add(timeSlotViewModel);

            await _mediator.Send(new LoadTimeSlotControlCommand
            {
                TimeSlotId = timeSlotDto.TimeSlotId.ToString(),
                ViewId = timeSlotViewModel.ViewId.ToString()
            });
        }

        if (TimeSlotViewModels.Any())
        {
            CurrentViewModelIndex = TimeSlotViewModels.Count - 1;
            SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
        }
    }

    private async Task CreateNewTimeSlotViewModel()
    {
        if (SelectedTicket == null) return;

        var timeSlotViewModel = _timeSlotViewModelFactory.Create();

        TimeSlotViewModels.Add(timeSlotViewModel);
        await _mediator.Send(new CreateTimeSlotControlCommand
        {
            TicketId = SelectedTicket.TicketId.ToString(),
            ViewId = timeSlotViewModel.ViewId.ToString()
        });

        SelectedTicketName = TimeSlotViewModels[CurrentViewModelIndex].Model.TicketReplayDecorator.Ticket.Name;
    }
}