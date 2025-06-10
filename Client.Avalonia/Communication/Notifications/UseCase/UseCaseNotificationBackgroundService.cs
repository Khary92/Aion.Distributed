using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Notifications.UseCase;
using Microsoft.Extensions.Hosting;

public class UseCaseNotificationBackgroundService(UseCaseNotificationReceiver notificationReceiver)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return notificationReceiver.StartListeningAsync(stoppingToken);
    }
}