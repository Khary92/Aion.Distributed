namespace Service.Admin.Web.Communication.NoteType;

public class NoteTypeNotificationHostedService(
    NoteTypeNotificationReceiver notifications) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notifications.SubscribeToNotifications(stoppingToken);
    }
}