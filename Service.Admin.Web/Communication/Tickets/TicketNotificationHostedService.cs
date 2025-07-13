namespace Service.Admin.Web.Communication.Tickets;

public class TicketNotificationHostedService(TicketNotificationsReceiver notificationsReceiver,
    ILogger<TicketNotificationHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Ticket Notification Service wird gestartet");
        await notificationsReceiver.SubscribeToNotifications(stoppingToken);
    }
}