using Proto.Notifications.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.TimerSettings;

public static class TimerSettingsExtensions
{
    public static TimerSettingsDto ToDto(this TimerSettingsCreatedNotification timerSettings)
    {
        return new TimerSettingsDto(Guid.Parse(timerSettings.TimerSettingsId), timerSettings.DocumentationSaveInterval,
            timerSettings.SnapshotSaveInterval);
    }

    public static WebDocuIntervalChangedNotification ToNotification(
        this DocuTimerSaveIntervalChangedNotification notification)
    {
        return new WebDocuIntervalChangedNotification(Guid.Parse(notification.TimerSettingsId),
            notification.DocuTimerSaveInterval);
    }

    public static WebSnapshotIntervalChangedNotification ToNotification(
        this SnapshotSaveIntervalChangedNotification notification)
    {
        return new WebSnapshotIntervalChangedNotification(Guid.Parse(notification.TimerSettingsId),
            notification.SnapshotSaveInterval);
    }
}