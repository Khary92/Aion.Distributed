using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Ticket;
using Service.Admin.Tracing;

namespace Service.Admin.Web.Communication.Tickets;

public class TicketNotificationsReceiver(
    ILogger<TicketNotificationsReceiver> logger,
    TicketHub ticketHub,
    ITraceCollector tracer)
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
        var client = new TicketNotificationService.TicketNotificationServiceClient(channel);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Starte Ticket-Notification Stream...");
                using var call =
                    client.SubscribeTicketNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case TicketNotification.NotificationOneofCase.TicketCreated:
                            var notificationTicketCreated = notification.TicketCreated;
                            await tracer.Ticket.Create.NotificationReceived(GetType(),
                                Guid.Parse(notificationTicketCreated.TicketId), notificationTicketCreated);
                            await ticketHub.ReceiveTicketCreated(notificationTicketCreated.ToDto());
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDataUpdated:
                            logger.LogInformation("Ticket aktualisiert: {TicketId}",
                                notification.TicketDataUpdated.TicketId);
                            await ticketHub.ReceiveTicketDataUpdate(notification.TicketDataUpdated.ToNotification());
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
                            logger.LogInformation("Ticket-Dokumentation aktualisiert: {TicketId}",
                                notification.TicketDocumentationUpdated.TicketId);
                            await ticketHub.ReceiveTicketDocumentationUpdated(notification.TicketDocumentationUpdated
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