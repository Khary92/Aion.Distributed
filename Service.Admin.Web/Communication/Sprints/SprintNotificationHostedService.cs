namespace Service.Admin.Web.Communication.Sprints;

public class SprintNotificationHostedService(
    SprintNotificationsReceiver notifications,
    ILogger<SprintNotificationHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Sprint Notification Service wird gestartet");
        await notifications.SubscribeToNotifications(stoppingToken);
    }
}