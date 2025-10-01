using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.Settings;

public class WorkDaysViewModel : ReactiveObject
{
    private WorkDayClientModel? _selectedWorkDay;

    public WorkDaysViewModel(WorkDaysModel workDaysModel)
    {
        Model = workDaysModel;

        LoadSelectedDateCommand =
            ReactiveCommand.CreateFromTask(SetSelectedWorkday, this.WhenAnyValue(x => x.SelectedWorkDay).Any());
    }

    public WorkDaysModel Model { get; }

    public ReactiveCommand<Unit, Unit>? LoadSelectedDateCommand { get; internal set; }

    public WorkDayClientModel? SelectedWorkDay
    {
        get => _selectedWorkDay;
        set => this.RaiseAndSetIfChanged(ref _selectedWorkDay, value);
    }

    private async Task SetSelectedWorkday()
    {
        if (_selectedWorkDay == null) return;

        await Model.SetSelectedWorkday(SelectedWorkDay!);
    }
}