using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.TimerSettings.State;

public interface ITimerSettingsStateService
{
    TimerSettingsWebModel TimerSettings { get; }
    event Action? OnStateChanged;
    Task SetTimerSettings(NewTimerSettingsMessage timerSettingsMessage);
    Task Apply(WebDocuIntervalChangedNotification notification);
    Task Apply(WebSnapshotIntervalChangedNotification notification);
}