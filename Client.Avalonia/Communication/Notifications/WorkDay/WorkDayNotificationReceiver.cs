using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.WorkDay;

namespace Client.Avalonia.Communication.Notifications.WorkDay;

public class WorkDayNotificationReceiver(
    WorkDayNotificationService.WorkDayNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeWorkDayNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            switch (notification.NotificationCase)
            {
                case WorkDayNotification.NotificationOneofCase.WorkDayCreated:
                    messenger.Send(new NewWorkDayMessage(new WorkDayDto(
                        Guid.Parse(notification.WorkDayCreated.WorkDayId),
                        DateTimeOffset.Parse(notification.WorkDayCreated.Date))));
                    break;
            }
        }
    }
}