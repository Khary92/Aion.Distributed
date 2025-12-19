using Global.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Tag;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Authentication;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;

namespace Service.Admin.Web.Communication.Receiver;

public class TagNotificationsReceiver
{
    private readonly ITagStateService _tagStateService;
    private readonly ITraceCollector _tracer;
    private readonly JwtService _jwtService;
    private readonly TagNotificationService.TagNotificationServiceClient _client;

    public TagNotificationsReceiver(
        ITagStateService tagStateService,
        ITraceCollector tracer,
        IGrpcUrlService grpcUrlBuilder,
        JwtService jwtService)
    {
        _tagStateService = tagStateService;
        _tracer = tracer;
        _jwtService = jwtService;

        var channel = GrpcChannel.ForAddress(grpcUrlBuilder.InternalToServerUrl,
            new GrpcChannelOptions
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
                },
                Credentials = ChannelCredentials.Insecure,
                UnsafeUseInsecureChannelCallCredentials = true
            });

        _client = new TagNotificationService.TagNotificationServiceClient(channel);
    }

    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var call = _client.SubscribeTagNotifications(
                    new SubscribeRequest(),
                    headers: new Metadata { { "Authorization", $"Bearer {_jwtService.Token}" } },
                    cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case TagNotification.NotificationOneofCase.TagCreated:
                            var newTagMessage = notification.TagCreated.ToWebModel();
                            await _tracer.Tag.Create.NotificationReceived(GetType(), newTagMessage.TraceId, notification.TagCreated);
                            await _tagStateService.AddTag(newTagMessage);
                            break;

                        case TagNotification.NotificationOneofCase.TagUpdated:
                            var webTagUpdatedNotification = notification.TagUpdated.ToNotification();
                            await _tracer.Tag.Update.NotificationReceived(GetType(), webTagUpdatedNotification.TraceId, notification.TagUpdated);
                            await _tagStateService.Apply(webTagUpdatedNotification);
                            break;

                        case TagNotification.NotificationOneofCase.None:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
}
