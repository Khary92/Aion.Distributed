using Client.Avalonia.ViewModels.Tracing;

namespace Client.Avalonia.ViewModels.Settings;

public class SettingsCompositeViewModel(
    SettingsViewModel settingsViewModel,
    AiSettingsViewModel aiSettingsViewModel,
    TimerSettingsViewModel timerSettingsViewModel,
    WorkDaysViewModel workDaysViewModel,
    TracingViewModel tracingViewModel)
{
    public TracingViewModel TracingViewModel { get; } = tracingViewModel;
    public SettingsViewModel SettingsViewModel { get; } = settingsViewModel;
    public AiSettingsViewModel AiSettingsViewModel { get; } = aiSettingsViewModel;
    public WorkDaysViewModel WorkDaysViewModel { get; } = workDaysViewModel;
    public TimerSettingsViewModel TimerSettingsViewModel { get; } = timerSettingsViewModel;
}