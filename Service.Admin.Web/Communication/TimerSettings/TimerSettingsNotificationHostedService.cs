namespace Service.Admin.Web.Communication.TimerSettings;

public class TimerSettingsNotificationHostedService(TimerSettingsNotificationsReceiver notifications)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notifications.SubscribeToNotifications(stoppingToken);
    }
}