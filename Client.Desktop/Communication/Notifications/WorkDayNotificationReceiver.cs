using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.WorkDay;

namespace Client.Desktop.Communication.Notifications;

public class WorkDayNotificationReceiver(
    WorkDayNotificationService.WorkDayNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeWorkDayNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
            switch (notification.NotificationCase)
            {
                case WorkDayNotification.NotificationOneofCase.WorkDayCreated:
                    Dispatcher.UIThread.Post(() =>
                    {
                        messenger.Send(new NewWorkDayMessage(new WorkDayClientModel(
                            Guid.Parse(notification.WorkDayCreated.WorkDayId),
                            notification.WorkDayCreated.Date.ToDateTimeOffset())));
                    });

                    break;
            }
    }
}