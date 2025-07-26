namespace Service.Admin.Web.Communication.TimerSettings;

public class TimerSettingsNotificationHostedService(
    TimerSettingsNotificationsReceiver notifications,
    ILogger<TimerSettingsNotificationHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("TimerSettings Notification Service wird gestartet");
        await notifications.SubscribeToNotifications(stoppingToken);
    }
}