using System;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using Proto.Notifications.TimerSettings;

namespace Client.Desktop.Communication.Notifications.TimerSettings;

public static class TimerSettingsExtensions
{
    public static ClientDocuTimerSaveIntervalChangedNotification
        ToClientNotification(this DocuTimerSaveIntervalChangedNotification notification)
    {
        return new ClientDocuTimerSaveIntervalChangedNotification(Guid.Parse(notification.TimerSettingsId),
            notification.DocuTimerSaveInterval, Guid.Parse(notification.TraceData.TraceId));
    }


    public static ClientSnapshotSaveIntervalChangedNotification
        ToClientNotification(this SnapshotSaveIntervalChangedNotification notification)
    {
        return new ClientSnapshotSaveIntervalChangedNotification(Guid.Parse(notification.TimerSettingsId),
            notification.SnapshotSaveInterval, Guid.Parse(notification.TraceData.TraceId));
    }


}