using Avalonia.ReactiveUI;
using Client.Desktop.Models.Settings;

namespace Client.Desktop.Views.Setting;

public partial class WorkDaysControl : ReactiveUserControl<WorkDaysViewModel>
{
    public WorkDaysControl()
    {
        InitializeComponent();
    }

    public WorkDaysControl(WorkDaysViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}