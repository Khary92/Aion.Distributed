using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;

namespace Client.Desktop.Communication.Notifications.Tag.Receiver;

public interface ILocalTagNotificationPublisher
{
    event Func<ClientTagUpdatedNotification, Task>? ClientTagUpdatedNotificationReceived;
    event Func<NewTagMessage, Task>? NewTagMessageNotificationReceived;

    Task Publish(NewTagMessage message);
    Task Publish(ClientTagUpdatedNotification notification);
}