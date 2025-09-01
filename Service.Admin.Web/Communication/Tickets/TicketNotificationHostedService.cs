namespace Service.Admin.Web.Communication.Tickets;

public class TicketNotificationHostedService(
    TicketNotificationsReceiver notificationsReceiver) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notificationsReceiver.SubscribeToNotifications(stoppingToken);
    }
}