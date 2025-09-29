using Global.Settings.Types;
using Global.Settings.UrlResolver;
using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.TimerSettings;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Services.State;
using SubscribeRequest = Proto.Notifications.TimerSettings.SubscribeRequest;

namespace Service.Admin.Web.Communication.Receiver;

public class TimerSettingsNotificationsReceiver(
    ITimerSettingsStateService timerSettingsStateService,
    ITraceCollector tracer, IGrpcUrlBuilder grpcUrlBuilder)
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

        var channel =
            GrpcChannel.ForAddress(
                grpcUrlBuilder
                    .From(ResolvingServices.WebAdmin)
                    .To(ResolvingServices.Server)
                    .BuildAddress(),
                channelOptions);
        
        var client = new TimerSettingsNotificationService.TimerSettingsNotificationServiceClient(channel);

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                using var call =
                    client.SubscribeTimerSettingsNotifications(new SubscribeRequest(),
                        cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                    switch (notification.NotificationCase)
                    {
                        case TimerSettingsNotification.NotificationOneofCase.DocuTimerSaveIntervalChanged:
                            var webDocuIntervalChangedNotification =
                                notification.DocuTimerSaveIntervalChanged.ToNotification();

                            await tracer.TimerSettings.ChangeDocuTimerInterval.NotificationReceived(GetType(),
                                webDocuIntervalChangedNotification.TraceId, notification.TimerSettingsCreated);

                            await timerSettingsStateService.Apply(webDocuIntervalChangedNotification);
                            break;

                        case TimerSettingsNotification.NotificationOneofCase.SnapshotSaveIntervalChanged:
                            var webSnapshotIntervalChangedNotification =
                                notification.SnapshotSaveIntervalChanged.ToNotification();

                            await tracer.TimerSettings.ChangeSnapshotInterval.NotificationReceived(GetType(),
                                webSnapshotIntervalChangedNotification.TraceId, notification.TimerSettingsCreated);

                            await timerSettingsStateService.Apply(webSnapshotIntervalChangedNotification);
                            break;
                        case TimerSettingsNotification.NotificationOneofCase.None:
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