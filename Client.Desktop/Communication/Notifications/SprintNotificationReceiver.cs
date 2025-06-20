using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Sprint;

namespace Client.Desktop.Communication.Notifications;

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
                switch (notification.NotificationCase)
                {
                    case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                    {
                        Dispatcher.UIThread.Post(() => { messenger.Send(notification.SprintActiveStatusSet); });
                        break;
                    }
                    case SprintNotification.NotificationOneofCase.SprintCreated:
                    {
                        var created = notification.SprintCreated;
                        var ticketGuids = new List<Guid>();
                        foreach (var id in created.TicketIds)
                            if (Guid.TryParse(id, out var guid))
                                ticketGuids.Add(guid);

                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(
                                new NewSprintMessage(new SprintDto(Guid.Parse(created.SprintId),
                                    created.Name, created.IsActive, created.StartTime.ToDateTimeOffset(),
                                    created.EndTime.ToDateTimeOffset(), ticketGuids)));
                        });
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