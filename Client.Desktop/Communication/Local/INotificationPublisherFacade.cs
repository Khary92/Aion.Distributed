using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Notifications.Client.Receiver;
using Client.Desktop.Communication.Notifications.Note.Receiver;
using Client.Desktop.Communication.Notifications.NoteType.Receiver;
using Client.Desktop.Communication.Notifications.Sprint.Receiver;
using Client.Desktop.Communication.Notifications.StatisticsData.Receiver;
using Client.Desktop.Communication.Notifications.Tag.Receiver;
using Client.Desktop.Communication.Notifications.Ticket.Receiver;
using Client.Desktop.Communication.Notifications.TimerSettings.Receiver;
using Client.Desktop.Communication.Notifications.WorkDay.Receiver;

namespace Client.Desktop.Communication.Local;

public interface INotificationPublisherFacade
{
    ILocalClientNotificationPublisher Client { get; }
    ILocalNoteNotificationPublisher Note { get; }
    ILocalNoteTypeNotificationPublisher NoteType { get; }
    ILocalSprintNotificationPublisher Sprint { get; }
    ILocalStatisticsDataNotificationPublisher StatisticsData { get; }
    ILocalTagNotificationPublisher Tag { get; }
    ILocalTicketNotificationPublisher Ticket { get; }
    ILocalTimerSettingsNotificationPublisher TimerSettings { get; }
    ILocalWorkDayNotificationPublisher WorkDay { get; }
}