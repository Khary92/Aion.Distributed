namespace Service.Admin.Web.Communication.Tags;

public class TagNotificationHostedService(
    TagNotificationsReceiver notificationsReceiver) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notificationsReceiver.SubscribeToNotifications(stoppingToken);
    }
}