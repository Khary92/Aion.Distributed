using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Tag;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tags.State;

namespace Service.Admin.Web.Communication.Tags;

public class TagNotificationsReceiver(ITraceCollector tracer, ITagStateService tagStateService)
{
    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        var channelOptions = new GrpcChannelOptions
        {
            HttpHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
            }
        };

        var channel = GrpcChannel.ForAddress("http://core-service:8080", channelOptions);
        var client = new TagNotificationService.TagNotificationServiceClient(channel);

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                using var call =
                    client.SubscribeTagNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                    switch (notification.NotificationCase)
                    {
                        case TagNotification.NotificationOneofCase.TagCreated:
                            var notificationTagCreated = notification.TagCreated;
                            await tagStateService.AddTicket(notificationTagCreated.ToWebModel());
                            break;

                        case TagNotification.NotificationOneofCase.TagUpdated:
                            tagStateService.Apply(notification.TagUpdated.ToNotification());
                            break;
                    }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                await Task.Delay(5000, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                await Task.Delay(5000, stoppingToken);
            }
    }
}