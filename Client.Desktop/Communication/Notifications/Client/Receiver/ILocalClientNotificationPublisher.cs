using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Client.Records;

namespace Client.Desktop.Communication.Notifications.Client.Receiver;

public interface ILocalClientNotificationPublisher
{
    event Func<ClientSprintSelectionChangedNotification, Task>? ClientSprintSelectionChangedNotificationReceived;
    event Func<ClientTrackingControlCreatedNotification, Task>? ClientTrackingControlCreatedNotificationReceived;
}