using Proto.Notifications.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.TimerSettings;

public static class TimerSettingsExtensions
{
    public static NewTimerSettingsMessage ToNewEntityMessage(this TimerSettingsCreatedNotification notification)
        => new(new TimerSettingsWebModel(Guid.Parse(notification.TimerSettingsId), notification.DocumentationSaveInterval,
            notification.SnapshotSaveInterval), Guid.Parse(notification.TraceData.TraceId));

    public static WebDocuIntervalChangedNotification ToNotification(
        this DocuTimerSaveIntervalChangedNotification notification) => new(
        Guid.Parse(notification.TimerSettingsId), notification.DocuTimerSaveInterval,
        Guid.Parse(notification.TraceData.TraceId));

    public static WebSnapshotIntervalChangedNotification ToNotification(
        this SnapshotSaveIntervalChangedNotification notification) => new(Guid.Parse(notification.TimerSettingsId),
        notification.SnapshotSaveInterval, Guid.Parse(notification.TraceData.TraceId));
}