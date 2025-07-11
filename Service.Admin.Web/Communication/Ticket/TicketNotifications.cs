using Grpc.Core;
using Grpc.Net.Client;
using Proto.Notifications.Ticket;

namespace Service.Admin.Web.Communication.Ticket;

public class TicketNotifications
{
    private readonly ILogger<TicketNotifications> _logger;

    public TicketNotifications(ILogger<TicketNotifications> logger)
    {
        _logger = logger;
    }

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
                _logger.LogInformation("Starte Ticket-Notification Stream...");
                using var call = client.SubscribeTicketNotifications(new SubscribeRequest(), cancellationToken: stoppingToken);

                await foreach (var notification in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    switch (notification.NotificationCase)
                    {
                        case TicketNotification.NotificationOneofCase.TicketCreated:
                            _logger.LogInformation("Neues Ticket erstellt: {TicketId}", notification.TicketCreated.TicketId);
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDataUpdated:
                            _logger.LogInformation("Ticket aktualisiert: {TicketId}", notification.TicketDataUpdated.TicketId);
                            break;

                        case TicketNotification.NotificationOneofCase.TicketDocumentationUpdated:
                            _logger.LogInformation("Ticket-Dokumentation aktualisiert: {TicketId}", notification.TicketDocumentationUpdated.TicketId);
                            break;
                    }
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                _logger.LogWarning("Verbindung zum Server verloren. Versuche Neuverbindung in 5 Sekunden...");
                await Task.Delay(5000, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Ticket-Notification Stream wird beendet");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Empfangen der Benachrichtigungen");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}