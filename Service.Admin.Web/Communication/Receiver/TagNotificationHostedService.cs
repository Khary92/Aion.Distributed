namespace Service.Admin.Web.Communication.Receiver;

public class TagNotificationHostedService(
    TagNotificationsReceiver notificationsReceiver) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notificationsReceiver.SubscribeToNotifications(stoppingToken);
    }
}