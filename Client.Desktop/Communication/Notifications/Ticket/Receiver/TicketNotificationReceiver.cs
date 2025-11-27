using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Ticket;

namespace Client.Desktop.Communication.Notifications.Ticket.Receiver;

public class TicketNotificationReceiver(
    IGrpcUrlService grpcUrlBuilder,
    ITraceCollector tracer) : ILocalTicketNotificationPublisher, IStreamClient
{
    public event Func<ClientTicketDataUpdatedNotification, Task>? TicketDataUpdatedNotificationReceived;

    public event Func<ClientTicketDocumentationUpdatedNotification, Task>?
        TicketDocumentationUpdatedNotificationReceived;

    public event Func<NewTicketMessage, Task>? NewTicketNotificationReceived;

    public async Task Publish(NewTicketMessage message)
    {
        if (NewTicketNotificationReceived == null)
            throw new InvalidOperationException("NewTicketMessage received but no forwarding receiver is set");

        await NewTicketNotificationReceived.Invoke(message);
    }

    public async Task Publish(ClientTicketDataUpdatedNotification notification)
    {
        if (TicketDataUpdatedNotificationReceived == null)
            throw new InvalidOperationException(
                "TicketDataUpdatedNotification received but no forwarding receiver is set");

        await TicketDataUpdatedNotificationReceived.Invoke(notification);
    }

    public async Task Publish(ClientTicketDocumentationUpdatedNotification notification)
    {
        if (TicketDocumentationUpdatedNotificationReceived == null)
            throw new InvalidOperationException(
                "TicketDocumentationUpdatedNotification received but no forwarding receiver is set");

        await TicketDocumentationUpdatedNotificationReceived.Invoke(notification);
    }


    public async Task StartListening(CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
            try
            {
                using var channel = GrpcChannel.ForAddress(grpcUrlBuilder.ClientToServerUrl);

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

    private async Task HandleNotificationReceived(TicketNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case TicketNotification.NotificationOneofCase.TicketCreated:
            {
                var notificationTicketCreated = notification.TicketCreated;

                await tracer.Ticket.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationTicketCreated.TraceData.TraceId),
                    notificationTicketCreated);

                await Publish(notificationTicketCreated.ToNewEntityMessage());

                break;
            }
            case TicketNotification.NotificationOneofCase.TicketDataUpdated:
            {
                var notificationTicketDataUpdated = notification.TicketDataUpdated;

                await tracer.Ticket.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationTicketDataUpdated.TraceData.TraceId),
                    notificationTicketDataUpdated);

                await Publish(notificationTicketDataUpdated.ToClientNotification());

                break;
            }
            case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
            {
                var notificationTicketDocumentationUpdated = notification.TicketDocumentationUpdated;

                await tracer.Ticket.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(notificationTicketDocumentationUpdated.TraceData.TraceId),
                    notificationTicketDocumentationUpdated);

                await Publish(notificationTicketDocumentationUpdated.ToClientNotification());

                break;
            }
            case TicketNotification.NotificationOneofCase.None:
                break;
        }
    }
}