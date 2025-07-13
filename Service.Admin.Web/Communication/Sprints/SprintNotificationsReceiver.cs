using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Sprint;
using SubscribeRequest = Proto.Notifications.Sprint.SubscribeRequest;

namespace Service.Admin.Web.Communication.Sprints;

public class SprintNotificationsReceiver(ILogger<SprintNotificationsReceiver> logger, SprintHub sprintHub)
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
                logger.LogInformation("Starte Sprint-Notification Stream...");
                using var call =
                    client.SubscribeSprintNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case SprintNotification.NotificationOneofCase.SprintCreated:
                            logger.LogInformation("New sprint created: {SprintId}",
                                notification.SprintCreated.SprintId);
                            await sprintHub.ReceiveSprintCreated(notification.SprintCreated.ToDto());
                            break;

                        case SprintNotification.NotificationOneofCase.SprintDataUpdated:
                            logger.LogInformation("Sprint data updated: {SprintId}",
                                notification.SprintDataUpdated.SprintId);
                            await sprintHub.ReceiveSprintDataUpdated(notification.SprintDataUpdated.ToNotification());
                            break;

                        case SprintNotification.NotificationOneofCase.SprintActiveStatusSet:
                            logger.LogInformation("Sprint active state set: {SprintId}",
                                notification.SprintActiveStatusSet.SprintId);
                            await sprintHub.ReceiveSprintActiveStatusSet(notification.SprintActiveStatusSet
                                .ToNotification());
                            break;

                        case SprintNotification.NotificationOneofCase.TicketAddedToActiveSprint:
                            logger.LogInformation("Ticket added to active sprint: {TicketId}",
                                notification.TicketAddedToActiveSprint.TicketId);
                            await sprintHub.ReceiveTicketAddedToActiveSprint(notification.TicketAddedToActiveSprint
                                .ToNotification());
                            break;

                        case SprintNotification.NotificationOneofCase.TicketAddedToSprint:
                            logger.LogInformation("Ticket added to sprint: {TicketId}",
                                notification.TicketAddedToSprint.TicketId);
                            await sprintHub.ReceiveTicketAddedToSprint(notification.TicketAddedToSprint
                                .ToNotification());
                            break;
                    }
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                logger.LogWarning("Verbindung zum Server verloren. Versuche Neuverbindung in 5 Sekunden...");
                await Task.Delay(5000, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Ticket-Notification Stream wird beendet");
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fehler beim Empfangen der Benachrichtigungen");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}