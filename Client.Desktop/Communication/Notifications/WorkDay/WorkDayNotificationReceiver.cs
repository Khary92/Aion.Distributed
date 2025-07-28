using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.WorkDay;

namespace Client.Desktop.Communication.Notifications.WorkDay;

public class WorkDayNotificationReceiver(
    WorkDayNotificationService.WorkDayNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeWorkDayNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case WorkDayNotification.NotificationOneofCase.WorkDayCreated:
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.WorkDayCreated.ToNewEntityMessage());
                            });

                            break;
                    }
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
    }
}