using System.Threading;
using System.Threading.Tasks;

namespace Client.Desktop.Communication.Notifications;

public class NotificationReceiverStarter(
    NoteNotificationReceiver noteNotificationReceiver,
    NoteTypeNotificationReceiver noteTypeNotificationReceiver,
    SprintNotificationReceiver sprintNotificationReceiver,
    TagNotificationReceiver tagNotificationReceiver,
    TicketNotificationReceiver ticketNotificationReceiver,
    TimerSettingsNotificationReceiver timerSettingsNotificationReceiver,
    UseCaseNotificationReceiver useCaseNotificationReceiver,
    WorkDayNotificationReceiver workDayNotificationReceiver) : INotificationReceiverStarter
{
    public Task Start()
    {
        var tasks = new[]
        {
            noteNotificationReceiver.StartListeningAsync(CancellationToken.None),
            noteTypeNotificationReceiver.StartListeningAsync(CancellationToken.None),
            sprintNotificationReceiver.StartListeningAsync(CancellationToken.None),
            tagNotificationReceiver.StartListeningAsync(CancellationToken.None),
            ticketNotificationReceiver.StartListeningAsync(CancellationToken.None),
            timerSettingsNotificationReceiver.StartListeningAsync(CancellationToken.None),
            useCaseNotificationReceiver.StartListeningAsync(CancellationToken.None),
            workDayNotificationReceiver.StartListeningAsync(CancellationToken.None)
        };

        return Task.WhenAll(tasks);
    }
}