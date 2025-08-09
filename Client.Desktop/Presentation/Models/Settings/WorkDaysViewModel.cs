using System.Reactive.Linq;
using Client.Desktop.DataModels;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Notifications.UseCase;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.Settings;

public class WorkDaysViewModel : ReactiveObject
{
    private WorkDayClientModel? _selectedWorkDay;

    public WorkDaysViewModel(IMessenger messenger, WorkDaysModel workDaysModel)
    {
        Model = workDaysModel;

        LoadSelectedDateCommand = ReactiveCommand.Create(
            () =>
            {
                Model.SetSelectedWorkday(SelectedWorkDay!);
                messenger.Send(new WorkDaySelectionChangedNotification());
            },
            this.WhenAnyValue(x => x.SelectedWorkDay).Any()
        );
    }

    public WorkDaysModel Model { get; }

    public ReactiveCommand<Unit, Unit>? LoadSelectedDateCommand { get; internal set; }

    public WorkDayClientModel? SelectedWorkDay
    {
        get => _selectedWorkDay;
        set => this.RaiseAndSetIfChanged(ref _selectedWorkDay, value);
    }
}