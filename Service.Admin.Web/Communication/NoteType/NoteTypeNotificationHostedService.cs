namespace Service.Admin.Web.Communication.NoteType;

public class NoteTypeNotificationHostedService(NoteTypeNotificationReceiver notifications,
    ILogger<NoteTypeNotificationHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NoteType Notification Service wird gestartet");
        await notifications.SubscribeToNotifications(stoppingToken);
    }
}