using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.State;
using SubscribeRequest = Proto.Notifications.TimerSettings.SubscribeRequest;

namespace Service.Admin.Web.Communication.TimerSettings;

public class TimerSettingsNotificationsReceiver(ITimerSettingsStateService timerSettingsStateService)
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
                        case TimerSettingsNotification.NotificationOneofCase.TimerSettingsCreated:
                            await timerSettingsStateService.SetTimerSettings(notification.TimerSettingsCreated.ToDto());
                            break;

                        case TimerSettingsNotification.NotificationOneofCase.DocuTimerSaveIntervalChanged:
                            timerSettingsStateService.Apply(notification.DocuTimerSaveIntervalChanged.ToNotification());
                            break;

                        case TimerSettingsNotification.NotificationOneofCase.SnapshotSaveIntervalChanged:
                            timerSettingsStateService.Apply(notification.SnapshotSaveIntervalChanged.ToNotification());
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