using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Services.State;

public interface ITimerSettingsStateService
{
    TimerSettingsWebModel TimerSettings { get; }
    event Action? OnStateChanged;
    Task Apply(WebDocuIntervalChangedNotification notification);
    Task Apply(WebSnapshotIntervalChangedNotification notification);
}