using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Proto.Notifications.Note;

namespace Client.Avalonia.Communication.Notifications;

public class NoteNotificationProcessor(IMessenger messenger)
{
    public Task Handle(NoteCreatedNotification notification, CancellationToken cancellationToken)
    {
        var note = new NoteDto(Guid.Parse(notification.NoteId), notification.Text, 
            Guid.Parse(notification.NoteTypeId),
            Guid.Parse(notification.TimeSlotId),
            DateTimeOffset.Parse(notification.TimeStamp));

        messenger.Send(new NewNoteMessage(note));

        return Task.CompletedTask;
    }

    public Task Handle(NoteUpdatedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }
}