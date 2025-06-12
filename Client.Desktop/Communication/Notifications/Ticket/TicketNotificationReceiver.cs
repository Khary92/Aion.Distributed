using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Grpc.Core;
using Proto.Notifications.Ticket;

namespace Client.Desktop.Communication.Notifications.Ticket;

public class TicketNotificationReceiver(
    TicketNotificationService.TicketNotificationServiceClient client,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTicketNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
            {
                switch (notification.NotificationCase)
                {
                    case TicketNotification.NotificationOneofCase.TicketCreated:
                    {
                        var created = notification.TicketCreated;
                        var sprintGuids = new Collection<Guid>();
                        foreach (var id in created.SprintIds)
                        {
                            if (Guid.TryParse(id, out var guid))
                                sprintGuids.Add(guid);
                        }

                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(new NewTicketMessage(new TicketDto(
                                Guid.Parse(created.TicketId),
                                created.Name,
                                created.BookingNumber,
                                string.Empty,
                                sprintGuids
                            )));
                        });
                        break;
                    }
                    case TicketNotification.NotificationOneofCase.TicketDataUpdated:
                    {
                        Dispatcher.UIThread.Post(() => messenger.Send(notification.TicketDataUpdated));
                        break;
                    }
                    case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
                    {
                        Dispatcher.UIThread.Post(() => messenger.Send(notification.TicketDocumentationUpdated));
                        break;
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Graceful shutdown
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in TicketNotificationReceiver: {ex}");
        }
    }
}