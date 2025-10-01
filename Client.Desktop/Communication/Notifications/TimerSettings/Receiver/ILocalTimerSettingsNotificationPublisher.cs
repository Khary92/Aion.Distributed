using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;

namespace Client.Desktop.Communication.Notifications.TimerSettings.Receiver;

public interface ILocalTimerSettingsNotificationPublisher
{
    event Func<ClientDocuTimerSaveIntervalChangedNotification, Task>?
        ClientDocuTimerSaveIntervalChangedNotificationReceived;

    event Func<ClientSnapshotSaveIntervalChangedNotification, Task>?
        ClientSnapshotSaveIntervalChangedNotificationReceived;

    Task Publish(ClientDocuTimerSaveIntervalChangedNotification notification);
    Task Publish(ClientSnapshotSaveIntervalChangedNotification notification);
}