using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Sprint;

namespace Client.Desktop.Communication.Notifications.Sprint;

public class SprintNotificationReceiver(
    SprintNotificationService.SprintNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeSprintNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case SprintNotification.NotificationOneofCase.SprintCreated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.SprintCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.SprintActiveStatusSet.ToClientNotification());
                            });
                            break;
                        }
                        case SprintNotification.NotificationOneofCase.SprintDataUpdated:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.SprintDataUpdated.ToClientNotification());
                            });
                            break;
                        }
                        case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.TicketAddedToActiveSprint.ToClientNotification());
                            });
                            break;
                        }
                        case SprintNotification.NotificationOneofCase.TicketAddedToSprint:
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.TicketAddedToSprint.ToClientNotification());
                            });
                            break;
                        }
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