using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using Proto.Notifications.TimerSettings;

namespace Client.Desktop.Communication.Notifications.TimerSettings;

public static class TimerSettingsExtensions
{
    public static ClientDocuTimerSaveIntervalChangedNotification
        ToClientNotification(this DocuTimerSaveIntervalChangedNotification notification)
    {
        return new ClientDocuTimerSaveIntervalChangedNotification(notification.DocuTimerSaveInterval);
    }


    public static ClientSnapshotSaveIntervalChangedNotification
        ToClientNotification(this SnapshotSaveIntervalChangedNotification notification)
    {
        return new ClientSnapshotSaveIntervalChangedNotification(notification.SnapshotSaveInterval);
    }
}