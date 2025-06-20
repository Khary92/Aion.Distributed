using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.TimeTracking;

//TODO reimplement
public class TimeTrackingViewModel : ReactiveObject
// INotificationHandler<SprintSelectionChangedNotification>,
// INotificationHandler<WorkDaySelectionChangedNotification>
{
    private readonly ICommandSender _commandSender;
    private readonly IRequestSender _requestSender;


    public TimeTrackingViewModel(ICommandSender commandSender, IRequestSender requestSender,
        TimeTrackingModel timeTrackingModel)
    {
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
}