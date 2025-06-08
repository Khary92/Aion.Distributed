using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.NoteType;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class NoteTypeNotificationProcessor(IMessenger messenger) :
    INotificationHandler<NoteTypeCreatedNotification>,
    INotificationHandler<NoteTypeColorChangedNotification>,
    INotificationHandler<NoteTypeNameChangedNotification>
{
    public Task Handle(NoteTypeColorChangedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);

        return Task.CompletedTask;
    }

    public Task Handle(NoteTypeCreatedNotification notification, CancellationToken cancellationToken)
    {
        var noteType = new NoteTypeDto(notification.NoteTypeId, notification.Name, notification.Color);

        messenger.Send(new NewNoteTypeMessage(noteType));

        return Task.CompletedTask;
    }

    public Task Handle(NoteTypeNameChangedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);

        return Task.CompletedTask;
    }
}