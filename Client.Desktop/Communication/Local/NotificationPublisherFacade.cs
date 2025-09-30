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

public class NotificationPublisherFacade(
    ILocalClientNotificationPublisher localClientNotificationPublisher,
    ILocalNoteNotificationPublisher localNoteNotificationPublisher,
    ILocalNoteTypeNotificationPublisher localNoteTypeNotificationPublisher,
    ILocalSprintNotificationPublisher localSprintNotificationPublisher,
    ILocalStatisticsDataNotificationPublisher localStatisticsDataNotificationPublisher,
    ILocalTagNotificationPublisher localTagNotificationPublisher,
    ILocalTicketNotificationPublisher localTicketNotificationPublisher,
    ILocalTimerSettingsNotificationPublisher localTimerSettingsNotificationPublisher,
    ILocalWorkDayNotificationPublisher localWorkDayNotificationPublisher) : INotificationPublisherFacade
{
    public ILocalClientNotificationPublisher Client => localClientNotificationPublisher;
    public ILocalNoteNotificationPublisher Note => localNoteNotificationPublisher;
    public ILocalNoteTypeNotificationPublisher NoteType => localNoteTypeNotificationPublisher;
    public ILocalSprintNotificationPublisher Sprint => localSprintNotificationPublisher;
    public ILocalStatisticsDataNotificationPublisher StatisticsData => localStatisticsDataNotificationPublisher;
    public ILocalTagNotificationPublisher Tag => localTagNotificationPublisher;
    public ILocalTicketNotificationPublisher Ticket  => localTicketNotificationPublisher;
    public ILocalTimerSettingsNotificationPublisher TimerSettings => localTimerSettingsNotificationPublisher;   
    public ILocalWorkDayNotificationPublisher WorkDay => localWorkDayNotificationPublisher;  
}