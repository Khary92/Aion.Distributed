using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.Wrappers;

namespace Client.Desktop.Communication.Notifications.Note.Receiver;

public interface ILocalNoteNotificationPublisher
{
    event Func<ClientNoteUpdatedNotification, Task>? ClientNoteUpdatedNotificationReceived;
    event Func<NewNoteMessage, Task>? NewNoteMessageReceived;

    Task Publish(NewNoteMessage message);
    Task Publish(ClientNoteUpdatedNotification notification);
}