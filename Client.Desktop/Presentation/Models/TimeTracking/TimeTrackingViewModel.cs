using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeTrackingViewModel : ReactiveObject
{
    public TimeTrackingViewModel(TimeTrackingModel timeTrackingModel)
    {
        Model = timeTrackingModel;
        AddTimeSlotControlCommand = ReactiveCommand.CreateFromTask(Model.CreateNewTimeSlotViewModel);
        NextViewModelCommand = ReactiveCommand.Create(Model.ToggleNextViewModel);
        PreviousViewModelCommand = ReactiveCommand.Create(Model.TogglePreviousViewModel);
    }

    public TimeTrackingModel Model { get; }
    public ReactiveCommand<Unit, Unit>? AddTimeSlotControlCommand { get; internal set; }
    public ReactiveCommand<Unit, Unit>? PreviousViewModelCommand { get; internal set; }
    public ReactiveCommand<Unit, Unit>? NextViewModelCommand { get; internal set; }
}