using System.Threading;
using System.Threading.Tasks;

namespace Client.Desktop.Communication.Notifications;

public class NotificationReceiverStarter(
    AiSettingsNotificationReceiver aiSettingsNotificationReceiver,
    AiSettingsNotificationReceiver settingsNotificationReceiver,
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
            aiSettingsNotificationReceiver.StartListeningAsync(CancellationToken.None),
            settingsNotificationReceiver.StartListeningAsync(CancellationToken.None),
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