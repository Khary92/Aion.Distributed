namespace Service.Admin.Web.Communication.Receiver;

public class SprintNotificationHostedService(SprintNotificationsReceiver notifications) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notifications.SubscribeToNotifications(stoppingToken);
    }
}