using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Client.Avalonia.Communication.Notifications.Tags;

public class TagNotificationBackgroundService(TagNotificationReceiver notificationReceiver)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return notificationReceiver.StartListeningAsync(stoppingToken);
    }
}