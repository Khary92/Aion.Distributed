using System.Threading;
using System.Threading.Tasks;

namespace Client.Desktop.Communication.Notifications;

public class NotificationReceiverStarter(
    NoteNotificationReceiver noteNotificationReceiver,
    NoteTypeNotificationReceiver noteTypeNotificationReceiver,
    SprintNotificationReceiver sprintNotificationReceiver,
    TagNotificationReceiver tagNotificationReceiver,
    TicketNotificationReceiver ticketNotificationReceiver,
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
            useCaseNotificationReceiver.StartListeningAsync(CancellationToken.None),
            workDayNotificationReceiver.StartListeningAsync(CancellationToken.None)
        };

        return Task.WhenAll(tasks);
    }
}