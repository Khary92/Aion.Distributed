using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Tag;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tags.State;

namespace Service.Admin.Web.Communication.Tags;

public class TagNotificationsReceiver(ITagStateService tagStateService, ITraceCollector tracer)
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
                            var newTagMessage = notification.TagCreated.ToWebModel();

                            await tracer.Tag.Create.NotificationReceived(GetType(), newTagMessage.TraceId,
                                notification.TagCreated);

                            await tagStateService.AddTag(newTagMessage);
                            break;

                        case TagNotification.NotificationOneofCase.TagUpdated:
                            var webTagUpdatedNotification = notification.TagUpdated.ToNotification();

                            await tracer.Tag.Update.NotificationReceived(GetType(), webTagUpdatedNotification.TraceId,
                                notification.TagCreated);

                            await tagStateService.Apply(webTagUpdatedNotification);
                            break;
                        case TagNotification.NotificationOneofCase.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
                Console.WriteLine(GetType() + " caused an error: " + ex);
                await Task.Delay(5000, stoppingToken);
            }
    }
}