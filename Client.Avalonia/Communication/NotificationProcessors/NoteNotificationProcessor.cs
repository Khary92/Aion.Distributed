using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Note;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class NoteNotificationProcessor(IMessenger messenger) :
    INotificationHandler<NoteCreatedNotification>,
    INotificationHandler<NoteUpdatedNotification>
{
    public Task Handle(NoteCreatedNotification notification, CancellationToken cancellationToken)
    {
        var note = new NoteDto(notification.NoteId, notification.Text, notification.NoteTypeId, notification.TimeSlotId,
            notification.TimeStamp);

        messenger.Send(new NewNoteMessage(note));

        return Task.CompletedTask;
    }

    public Task Handle(NoteUpdatedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }
}