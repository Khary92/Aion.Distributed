namespace Client.Desktop.Models.Settings;

public class SettingsCompositeViewModel(
    SettingsViewModel settingsViewModel,
    WorkDaysViewModel workDaysViewModel)
{
    public SettingsViewModel SettingsViewModel { get; } = settingsViewModel;
    public WorkDaysViewModel WorkDaysViewModel { get; } = workDaysViewModel;
}