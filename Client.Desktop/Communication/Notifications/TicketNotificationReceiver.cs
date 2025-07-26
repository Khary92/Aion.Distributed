using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Notifications.NotificationWrappers;
using Client.Desktop.DataModels;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Ticket;

namespace Client.Desktop.Communication.Notifications;

public class TicketNotificationReceiver(
    TicketNotificationService.TicketNotificationServiceClient client,
    ITraceCollector tracer,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTicketNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        try
        {
            await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                switch (notification.NotificationCase)
                {
                    case TicketNotification.NotificationOneofCase.TicketCreated:
                    {
                        var created = notification.TicketCreated;

                        await tracer.Ticket.Create.NotificationReceived(GetType(), Guid.Parse(created.TicketId),
                            created);

                        var sprintGuids = new List<Guid>();
                        foreach (var id in created.SprintIds)
                            if (Guid.TryParse(id, out var guid))
                                sprintGuids.Add(guid);

                        Dispatcher.UIThread.Post(() =>
                        {
                            messenger.Send(new NewTicketMessage(new TicketClientModel(
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