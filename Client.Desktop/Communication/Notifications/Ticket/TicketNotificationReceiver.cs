using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Core;
using Proto.Notifications.Ticket;

namespace Client.Desktop.Communication.Notifications.Ticket;

public class TicketNotificationReceiver(
    TicketNotificationService.TicketNotificationServiceClient client,
    ITraceCollector tracer,
    IMessenger messenger)
{
    public async Task StartListeningAsync(CancellationToken cancellationToken)
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
                            Dispatcher.UIThread.Post(() =>
                            {
                                messenger.Send(notification.TicketCreated.ToNewEntityMessage());
                            });
                            break;
                        }
                        case TicketNotification.NotificationOneofCase.TicketDataUpdated:
                        {
                            Dispatcher.UIThread.Post(() =>
                                messenger.Send(notification.TicketDataUpdated.ToClientNotification()));
                            break;
                        }
                        case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
                        {
                            Dispatcher.UIThread.Post(() =>
                                messenger.Send(notification.TicketDocumentationUpdated.ToClientNotification()));
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