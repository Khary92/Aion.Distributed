using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Tracing.Tracing.Tracers;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Tag;

namespace Client.Desktop.Communication.Notifications.Tag.Receiver;

public class TagNotificationReceiver(
    IGrpcUrlBuilder grpcUrlBuilder,
    ITraceCollector tracer) : ILocalTagNotificationPublisher
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

                var client = new TagNotificationService.TagNotificationServiceClient(channel);
                using var call =
                    client.SubscribeTagNotifications(new SubscribeRequest(), cancellationToken: cancellationToken);

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

    private async Task HandleNotificationReceived(TagNotification notification)
    {
        switch (notification.NotificationCase)
        {
            case TagNotification.NotificationOneofCase.TagCreated:
            {
                var n = notification.TagCreated;

                await tracer.Tag.Create.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (NewTagMessageNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await NewTagMessageNotificationReceived.Invoke(n.ToNewEntityMessage());
                
                break;
            }
            case TagNotification.NotificationOneofCase.TagUpdated:
            {
                var n = notification.TagUpdated;

                await tracer.Tag.Update.NotificationReceived(
                    GetType(),
                    Guid.Parse(n.TraceData.TraceId),
                    n);

                if (ClientTagUpdatedNotificationReceived == null)
                {
                    throw new InvalidOperationException(
                        "Ticket data update received but no forwarding receiver is set");
                }

                await ClientTagUpdatedNotificationReceived.Invoke(n.ToClientNotification());
                
                break;
            }
            case TagNotification.NotificationOneofCase.None:
                break;
        }
    }

    public event Func<ClientTagUpdatedNotification, Task>? ClientTagUpdatedNotificationReceived;
    public event Func<NewTagMessage, Task>? NewTagMessageNotificationReceived;
}