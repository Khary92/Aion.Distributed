namespace Service.Admin.Web.Communication.Ticket;

public class TicketNotificationHostedService : BackgroundService
{
    private readonly TicketNotifications _notifications;
    private readonly ILogger<TicketNotificationHostedService> _logger;

    public TicketNotificationHostedService(
        TicketNotifications notifications,
        ILogger<TicketNotificationHostedService> logger)
    {
        _notifications = notifications;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Ticket Notification Service wird gestartet");
        await _notifications.SubscribeToNotifications(stoppingToken);
    }
}