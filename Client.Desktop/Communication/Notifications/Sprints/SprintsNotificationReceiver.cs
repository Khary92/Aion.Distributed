using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Sprint;

namespace Client.Desktop.Communication.Notifications.Sprints;

public class SprintNotificationReceiver(
    SprintNotificationService.SprintNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeSprintNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
            {
                switch (notification.NotificationCase)
                {
                    case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.SprintActiveStatusSet); });
                        break;
                    }
                    case SprintNotification.NotificationOneofCase.SprintCreated:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.SprintCreated); });
                        break;
                    }
                    case SprintNotification.NotificationOneofCase.SprintDataUpdated:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.SprintDataUpdated); });
                        break;
                    }
                    case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.TicketAddedToActiveSprint); });
                        break;
                    }
                    case SprintNotification.NotificationOneofCase.TicketAddedToSprint:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.TicketAddedToSprint); });
                        break;
                    }
                    default:
                        // Optional: handle unknown notification case or log
                        break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Graceful cancellation
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SprintNotificationReceiver: {ex}");
        }
    }
}