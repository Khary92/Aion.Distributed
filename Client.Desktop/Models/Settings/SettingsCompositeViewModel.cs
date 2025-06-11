namespace Client.Desktop.Models.Settings;

public class SettingsCompositeViewModel(
    SettingsViewModel settingsViewModel,
    AiSettingsViewModel aiSettingsViewModel,
    TimerSettingsViewModel timerSettingsViewModel,
    WorkDaysViewModel workDaysViewModel)
{
    public SettingsViewModel SettingsViewModel { get; } = settingsViewModel;
    public AiSettingsViewModel AiSettingsViewModel { get; } = aiSettingsViewModel;
    public WorkDaysViewModel WorkDaysViewModel { get; } = workDaysViewModel;
    public TimerSettingsViewModel TimerSettingsViewModel { get; } = timerSettingsViewModel;
}