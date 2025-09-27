namespace Service.Admin.Web.Communication.Receiver;

public class TicketNotificationHostedService(
    TicketNotificationsReceiver notificationsReceiver) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notificationsReceiver.SubscribeToNotifications(stoppingToken);
    }
}