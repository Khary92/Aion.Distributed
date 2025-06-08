using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Tags;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class TagNotificationProcessor(IMessenger messenger) :
    INotificationHandler<TagCreatedNotification>,
    INotificationHandler<TagUpdatedNotification>
{
    public Task Handle(TagCreatedNotification notification, CancellationToken cancellationToken)
    {
        var tag = new TagDto(notification.TagId, notification.Name, false);

        messenger.Send(new NewTagMessage(tag));
        return Task.CompletedTask;
    }

    public Task Handle(TagUpdatedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }
}