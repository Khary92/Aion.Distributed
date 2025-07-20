using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Sprint;
using Service.Admin.Web.Communication.Sprints.State;
using SubscribeRequest = Proto.Notifications.Sprint.SubscribeRequest;

namespace Service.Admin.Web.Communication.Sprints;

public class SprintNotificationsReceiver(ISprintStateService sprintStateService)
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
        var client = new SprintNotificationService.SprintNotificationServiceClient(channel);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var call =
                    client.SubscribeSprintNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case SprintNotification.NotificationOneofCase.SprintCreated:
                            await sprintStateService.AddSprint(notification.SprintCreated.ToDto());
                            break;

                        case SprintNotification.NotificationOneofCase.SprintDataUpdated:
                            sprintStateService.Apply(notification.SprintDataUpdated.ToNotification());
                            break;

                        case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                            sprintStateService.Apply(notification.SprintDataUpdated.ToNotification());
                            break;

                        case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
                            sprintStateService.Apply(notification.SprintDataUpdated.ToNotification());
                            break;

                        case SprintNotification.NotificationOneofCase.TicketAddedToSprint:
                            sprintStateService.Apply(notification.SprintDataUpdated.ToNotification());
                            break;
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
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}