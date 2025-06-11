using Avalonia.ReactiveUI;
using Client.Desktop.Models.Settings;

namespace Client.Desktop.Views.Setting;

public partial class TimerSettingsControl : ReactiveUserControl<TimerSettingsViewModel>
{
    public TimerSettingsControl()
    {
        InitializeComponent();
    }

    public TimerSettingsControl(TimerSettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}