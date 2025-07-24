using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.TimerSettings.State;

public interface ITimerSettingsStateService
{
    TimerSettingsDto TimerSettings { get; }
    event Action? OnStateChanged;
    Task SetTimerSettings(TimerSettingsDto timerSettings);
    Task LoadSettings();
    void Apply(WebDocuIntervalChangedNotification notification);
    void Apply(WebSnapshotIntervalChangedNotification notification);
}