using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Sprints;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class SprintNotificationProcessor(IMessenger messenger) :
    INotificationHandler<SprintCreatedNotification>,
    INotificationHandler<SprintDataUpdatedNotification>,
    INotificationHandler<TicketAddedToSprintNotification>,
    INotificationHandler<SprintActiveStatusSetNotification>,
    INotificationHandler<TicketAddedToActiveSprintNotification>
{
    public Task Handle(SprintActiveStatusSetNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);

        return Task.CompletedTask;
    }

    public Task Handle(SprintCreatedNotification notification, CancellationToken cancellationToken)
    {
        var sprint = new SprintDto(notification.SprintId, notification.Name, notification.IsActive,
            notification.StartTime, notification.EndTime,
            notification.TicketIds);

        messenger.Send(new NewSprintMessage(sprint));

        return Task.CompletedTask;
    }

    public Task Handle(SprintDataUpdatedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);

        return Task.CompletedTask;
    }

    public Task Handle(TicketAddedToActiveSprintNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);

        return Task.CompletedTask;
    }

    public Task Handle(TicketAddedToSprintNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);

        return Task.CompletedTask;
    }
}