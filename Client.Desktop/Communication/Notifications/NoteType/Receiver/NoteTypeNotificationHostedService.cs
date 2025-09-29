using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Client.Desktop.Communication.Notifications.NoteType.Receiver;

public class NoteTypeNotificationHostedService(
    NoteTypeNotificationReceiver notifications) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await notifications.StartListening(stoppingToken);
    }
}