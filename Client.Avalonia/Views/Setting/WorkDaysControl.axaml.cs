using Avalonia.ReactiveUI;
using Client.Avalonia.ViewModels.Settings;

namespace Client.Avalonia.Views.Setting;

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