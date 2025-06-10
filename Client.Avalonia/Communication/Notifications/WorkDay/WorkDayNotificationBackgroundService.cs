using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Client.Avalonia.Communication.Notifications.WorkDay;

public class WorkDayNotificationBackgroundService(WorkDayNotificationReceiver notificationReceiver)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return notificationReceiver.StartListeningAsync(stoppingToken);
    }
}