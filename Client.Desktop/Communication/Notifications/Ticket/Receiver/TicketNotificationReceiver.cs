using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Ticket;

namespace Client.Desktop.Communication.Notifications.Ticket.Receiver;

public class TicketNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    IMessenger messenger,
    ITraceCollector tracer) : ILocalTicketNotificationPublisher
{
    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(grpcUrlBuilder
                    .From(ResolvingServices.Client)
                    .To(ResolvingServices.Server)
                    .BuildAddress());

                var client = new TicketNotificationService.TicketNotificationServiceClient(channel);
                using var call =
                    client.SubscribeTicketNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

                attempt = 0;

                await foreach (var notification in call.ResponseStream.ReadAllAsync(cancellationToken))
                    await HandleNotificationReceived(notification);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled &&
                                          cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType()} caused an exception: {ex}");

                if (cancellationToken.IsCancellationRequested)
                    return;

                attempt++;
                var backoffSeconds = Math.Min(30, Math.Pow(2, attempt));
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(backoffSeconds), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }
    }

    private async Task HandleNotificationReceived(TicketNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case TicketNotification.NotificationOneofCase.TicketCreated:
            {
                var n = notification.TicketCreated;

                await tracer.Ticket.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (NewTicketNotificationReceived == null)
                {
                    throw new InvalidOperationException("New ticket received but no forwarding receiver is set");
                }

                await NewTicketNotificationReceived.Invoke(n.ToNewEntityMessage());

                break;
            }
            case TicketNotification.NotificationOneofCase.TicketDataUpdated:
            {
                var n = notification.TicketDataUpdated;

                await tracer.Ticket.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (TicketDataUpdatedNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await TicketDataUpdatedNotificationReceived.Invoke(n.ToClientNotification());

                break;
            }
            case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
            {
                var n = notification.TicketDocumentationUpdated;

                await tracer.Ticket.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (TicketDocumentationUpdatedNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket documentation update received but no forwarding receiver is set");
                }

                await TicketDocumentationUpdatedNotificationReceived.Invoke(n.ToClientNotification());

                break;
            }
            case TicketNotification.NotificationOneofCase.None:
                break;
        }
    }

    public event Func<ClientTicketDataUpdatedNotification, Task>? TicketDataUpdatedNotificationReceived;

    public event Func<ClientTicketDocumentationUpdatedNotification, Task>?
        TicketDocumentationUpdatedNotificationReceived;

    public event Func<NewTicketMessage, Task>? NewTicketNotificationReceived;
}