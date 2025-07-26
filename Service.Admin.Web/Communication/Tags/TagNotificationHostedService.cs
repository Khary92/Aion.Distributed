namespace Service.Admin.Web.Communication.Tags;

public class TagNotificationHostedService(
    TagNotificationsReceiver notificationsReceiver,
    ILogger<TagNotificationHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Tag notification service started");
        await notificationsReceiver.SubscribeToNotifications(stoppingToken);
    }
}