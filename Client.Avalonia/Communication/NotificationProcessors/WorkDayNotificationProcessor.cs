using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.WorkDays;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class WorkDayNotificationProcessor(IMessenger messenger) : INotificationHandler<WorkDayCreatedNotification>
{
    public Task Handle(WorkDayCreatedNotification notification, CancellationToken cancellationToken)
    {
        var workDay = new WorkDayDto(notification.WorkDayId, notification.Date);

        messenger.Send(new NewWorkDayMessage(workDay));
        return Task.CompletedTask;
    }
}