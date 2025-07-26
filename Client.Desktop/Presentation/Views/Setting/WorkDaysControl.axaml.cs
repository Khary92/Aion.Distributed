using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Models.Settings;

namespace Client.Desktop.Presentation.Views.Setting;

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