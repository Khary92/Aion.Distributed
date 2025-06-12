using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Notes;
using Client.Desktop.Communication.Notifications.NoteType;
using Client.Desktop.Communication.Notifications.Sprints;
using Client.Desktop.Communication.Notifications.Tags;
using Client.Desktop.Communication.Notifications.Ticket;
using Client.Desktop.Communication.Notifications.TimerSettings;
using Client.Desktop.Communication.Notifications.UseCase;
using Client.Desktop.Communication.Notifications.WorkDay;

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
    public async Task Start()
    {
        await noteNotificationReceiver.StartListeningAsync(CancellationToken.None);
        await noteTypeNotificationReceiver.StartListeningAsync(CancellationToken.None);
        await sprintNotificationReceiver.StartListeningAsync(CancellationToken.None);
        await tagNotificationReceiver.StartListeningAsync(CancellationToken.None);
        await ticketNotificationReceiver.StartListeningAsync(CancellationToken.None);
        await timerSettingsNotificationReceiver.StartListeningAsync(CancellationToken.None);
        await useCaseNotificationReceiver.StartListeningAsync(CancellationToken.None);
        await workDayNotificationReceiver.StartListeningAsync(CancellationToken.None);
    }
}