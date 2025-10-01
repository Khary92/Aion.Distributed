using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Wrappers;

namespace Client.Desktop.Communication.Notifications.NoteType.Receiver;

public interface ILocalNoteTypeNotificationPublisher
{
    event Func<ClientNoteTypeColorChangedNotification, Task>? ClientNoteTypeColorChangedNotificationReceived;
    event Func<ClientNoteTypeNameChangedNotification, Task>? ClientNoteTypeNameChangedNotificationReceived;
    event Func<NewNoteTypeMessage, Task>? NewNoteTypeMessageReceived;

    Task Publish(NewNoteTypeMessage message);
    Task Publish(ClientNoteTypeColorChangedNotification notification);
    Task Publish(ClientNoteTypeNameChangedNotification notification);
}