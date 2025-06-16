using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using MediatR;
using Proto.Notifications.UseCase;
using Proto.Requests.Sprints;
using Proto.Requests.Tickets;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.TimeTracking;

//TODO reimplement
public class TimeTrackingViewModel : ReactiveObject
// INotificationHandler<SprintSelectionChangedNotification>,
// INotificationHandler<TimeSlotControlCreatedNotification>,
// INotificationHandler<WorkDaySelectionChangedNotification>
{
    // Mediator needs to be removed...
    private readonly IMediator _mediator;
    private readonly ICommandSender _commandSender;
    private readonly IRequestSender _requestSender;


    public TimeTrackingViewModel(IMediator mediator,
        TimeTrackingModel timeTrackingModel, ICommandSender commandSender, IRequestSender requestSender)
    {
        _mediator = mediator;
        _commandSender = commandSender;
        _requestSender = requestSender;

        Model = timeTrackingModel;
        AddTimeSlotControlCommand =
            ReactiveCommand.CreateFromTask(Model.CreateNewTimeSlotViewModel);

        NextViewModelCommand = ReactiveCommand.Create(Model.ToggleNextViewModel);
        PreviousViewModelCommand = ReactiveCommand.Create(Model.TogglePreviousViewModel);

        Model.Initialize().ConfigureAwait(false);
        Model.RegisterMessenger();
    }


    public TimeTrackingModel Model { get; }

    public ReactiveCommand<Unit, Unit> AddTimeSlotControlCommand { get; }
    public ReactiveCommand<Unit, Unit> PreviousViewModelCommand { get; }
    public ReactiveCommand<Unit, Unit> NextViewModelCommand { get; }

    public async Task Handle(SprintSelectionChangedNotification notification, CancellationToken cancellationToken)
    {
        Model.FilteredTickets.Clear();

        var currentSprint = await _requestSender.Send(new GetActiveSprintRequestProto());

        if (currentSprint == null) throw new InvalidOperationException("No active sprint");

        var ticketDtos = await _requestSender.Send(new GetAllTicketsRequestProto());
        foreach (var modelTicket in ticketDtos.Where(modelTicket =>
                     modelTicket.SprintIds.Contains(currentSprint.SprintId)))
            Model.FilteredTickets.Add(modelTicket);
    }

    public async Task Handle(WorkDaySelectionChangedNotification notification, CancellationToken cancellationToken)
    {
        Model.FilteredTickets.Clear();

        var currentSprint = await _mediator.Send(new GetActiveSprintRequest(), cancellationToken);

        if (currentSprint == null) throw new InvalidOperationException("No active sprint");

        var ticketDtos = await _mediator.Send(new GetAllTicketsRequest(), cancellationToken);

        foreach (var ticket in ticketDtos.Where(modelTicket => modelTicket.SprintIds.Contains(currentSprint.SprintId)))
            Model.FilteredTickets.Add(ticket);

        Model.TimeSlotViewModels.Clear();
        await Model.LoadTimeSlotViewModels();
    }
}