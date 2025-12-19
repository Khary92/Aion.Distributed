using Global.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Sprint;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Authentication;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;
using SubscribeRequest = Proto.Notifications.Sprint.SubscribeRequest;

namespace Service.Admin.Web.Communication.Receiver;

public class SprintNotificationsReceiver
{
    private readonly ISprintStateService _sprintStateService;
    private readonly ITraceCollector _tracer;
    private readonly JwtService _jwtService;
    private readonly SprintNotificationService.SprintNotificationServiceClient _client;

    public SprintNotificationsReceiver(
        ISprintStateService sprintStateService,
        ITraceCollector tracer,
        IGrpcUrlService grpcUrlBuilder,
        JwtService jwtService)
    {
        _sprintStateService = sprintStateService;
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

        _client = new SprintNotificationService.SprintNotificationServiceClient(channel);
    }

    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var call = _client.SubscribeSprintNotifications(
                    new SubscribeRequest(),
                    headers: new Metadata { { "Authorization", $"Bearer {_jwtService.Token}" } },
                    cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case SprintNotification.NotificationOneofCase.SprintCreated:
                            var newSprintMessage = notification.SprintCreated.ToWebModel();
                            await _tracer.Sprint.Create.NotificationReceived(GetType(), newSprintMessage.TraceId, notification);
                            await _sprintStateService.AddSprint(newSprintMessage);
                            break;

                        case SprintNotification.NotificationOneofCase.SprintDataUpdated:
                            var webSprintDataUpdatedNotification = notification.SprintDataUpdated.ToNotification();
                            await _tracer.Sprint.Update.NotificationReceived(GetType(), webSprintDataUpdatedNotification.TraceId, notification);
                            await _sprintStateService.Apply(webSprintDataUpdatedNotification);
                            break;

                        case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                            var webSetSprintActiveStatusNotification = notification.SprintActiveStatusSet.ToNotification();
                            await _tracer.Sprint.ActiveStatus.NotificationReceived(GetType(), webSetSprintActiveStatusNotification.TraceId, notification);
                            await _sprintStateService.Apply(webSetSprintActiveStatusNotification);
                            break;

                        case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
                            var webTicketAddedToActiveSprintNotification = notification.TicketAddedToActiveSprint.ToNotification();
                            await _tracer.Sprint.AddTicketToSprint.NotificationReceived(GetType(), webTicketAddedToActiveSprintNotification.TraceId, notification);
                            await _sprintStateService.Apply(webTicketAddedToActiveSprintNotification);
                            break;

                        case SprintNotification.NotificationOneofCase.None:
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
