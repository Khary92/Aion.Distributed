using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Client.Avalonia.Communication.Notifications.UseCase;

public class UseCaseNotificationBackgroundService(UseCaseNotificationReceiver notificationReceiver)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return notificationReceiver.StartListeningAsync(stoppingToken);
    }
}