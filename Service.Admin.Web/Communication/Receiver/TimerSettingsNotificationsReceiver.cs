using Global.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.TimerSettings;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Authentication;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;
using SubscribeRequest = Proto.Notifications.TimerSettings.SubscribeRequest;

namespace Service.Admin.Web.Communication.Receiver;

public class TimerSettingsNotificationsReceiver
{
    private readonly ITimerSettingsStateService _timerSettingsStateService;
    private readonly ITraceCollector _tracer;
    private readonly JwtService _jwtService;
    private readonly TimerSettingsNotificationService.TimerSettingsNotificationServiceClient _client;

    public TimerSettingsNotificationsReceiver(
        ITimerSettingsStateService timerSettingsStateService,
        ITraceCollector tracer,
        IGrpcUrlService grpcUrlBuilder,
        JwtService jwtService)
    {
        _timerSettingsStateService = timerSettingsStateService;
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

        _client = new TimerSettingsNotificationService.TimerSettingsNotificationServiceClient(channel);
    }

    public async Task SubscribeToNotifications(CancellationToken stoppingToken = default)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var call = _client.SubscribeTimerSettingsNotifications(
                    new SubscribeRequest(),
                    headers: new Metadata { { "Authorization", $"Bearer {_jwtService.Token}" } },
                    cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case TimerSettingsNotification.NotificationOneofCase.DocuTimerSaveIntervalChanged:
                            var webDocuIntervalChangedNotification =
                                notification.DocuTimerSaveIntervalChanged.ToNotification();

                            await _tracer.TimerSettings.ChangeDocuTimerInterval.NotificationReceived(
                                GetType(),
                                webDocuIntervalChangedNotification.TraceId,
                                notification.TimerSettingsCreated);

                            await _timerSettingsStateService.Apply(webDocuIntervalChangedNotification);
                            break;

                        case TimerSettingsNotification.NotificationOneofCase.SnapshotSaveIntervalChanged:
                            var webSnapshotIntervalChangedNotification =
                                notification.SnapshotSaveIntervalChanged.ToNotification();

                            await _tracer.TimerSettings.ChangeSnapshotInterval.NotificationReceived(
                                GetType(),
                                webSnapshotIntervalChangedNotification.TraceId,
                                notification.TimerSettingsCreated);

                            await _timerSettingsStateService.Apply(webSnapshotIntervalChangedNotification);
                            break;

                        case TimerSettingsNotification.NotificationOneofCase.None:
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
