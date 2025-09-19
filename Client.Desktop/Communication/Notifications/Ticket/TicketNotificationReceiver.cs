using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Ticket;

namespace Client.Desktop.Communication.Notifications.Ticket;

public class TicketNotificationReceiver(
    TicketNotificationService.TicketNotificationServiceClient client,
    IMessenger messenger,
    ITraceCollector tracer) : IStreamClient
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var call =
            client.SubscribeTicketNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    switch (notification.NotificationCase)
                    {
                        case TicketNotification.NotificationOneofCase.TicketCreated:
                        {
                            var notificationTicketCreated = notification.TicketCreated;

                            await tracer.Ticket.Create.NotificationReceived(GetType(),
                                Guid.Parse(notificationTicketCreated.TraceData.TraceId), notificationTicketCreated);
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationTicketCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case TicketNotification.NotificationOneofCase.TicketDataUpdated:
                        {
                            var notificationTicketDataUpdated = notification.TicketDataUpdated;
                            await tracer.Ticket.Update.NotificationReceived(GetType(),
                                Guid.Parse(notificationTicketDataUpdated.TraceData.TraceId),
                                notificationTicketDataUpdated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationTicketDataUpdated.ToClientNotification());
                            });
                            break;
                        }
                        case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
                        {
                            var notificationTicketDocumentationUpdated = notification.TicketDocumentationUpdated;

                            await tracer.Ticket.Update.NotificationReceived(GetType(),
                                Guid.Parse(notificationTicketDocumentationUpdated.TraceData.TraceId),
                                notificationTicketDocumentationUpdated);

                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notificationTicketDocumentationUpdated.ToClientNotification());
                            });
                            break;
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GetType() + " caused an exception: " + ex);

                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
    }
}