using Avalonia.ReactiveUI;
using Client.Avalonia.Models.Settings;

namespace Client.Avalonia.Views.Setting;

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