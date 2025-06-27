using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.TimeTracking;

public class TimeTrackingViewModel : ReactiveObject
{
    public TimeTrackingViewModel(TimeTrackingModel timeTrackingModel)
    {
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