using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Notifications.Client.Receiver;
using Client.Desktop.Communication.Notifications.Note.Receiver;
using Client.Desktop.Communication.Notifications.NoteType.Receiver;
using Client.Desktop.Communication.Notifications.Sprint.Receiver;
using Client.Desktop.Communication.Notifications.StatisticsData.Receiver;
using Client.Desktop.Communication.Notifications.Tag.Receiver;
using Client.Desktop.Communication.Notifications.Ticket.Receiver;
using Client.Desktop.Communication.Notifications.TimerSettings.Receiver;
using Client.Desktop.Communication.Notifications.WorkDay.Receiver;
using Client.Tracing.Tracing.Tracers;
using Global.Settings.UrlResolver;

namespace Client.Desktop.Test;

public class TestNotificationPublisherFacade(IGrpcUrlBuilder urlBuilder, ITraceCollector traceCollector)
    : INotificationPublisherFacade
{
    public ClientNotificationReceiver Client { get; } = new(urlBuilder, traceCollector);
    public NoteNotificationReceiver Note { get; } = new(urlBuilder, traceCollector);
    public NoteTypeNotificationReceiver NoteType { get; } = new(urlBuilder, traceCollector);
    public SprintNotificationReceiver Sprint { get; } = new(urlBuilder, traceCollector);
    public StatisticsDataNotificationReceiver StatisticsData { get; } = new(urlBuilder, traceCollector);
    public TagNotificationReceiver Tag { get; } = new(urlBuilder, traceCollector);
    public TicketNotificationReceiver Ticket { get; } = new(urlBuilder, traceCollector);
    public TimerSettingsNotificationReceiver TimerSettings { get; } = new(urlBuilder);
    public WorkDayNotificationReceiver WorkDay { get; } = new(urlBuilder, traceCollector);

    ILocalClientNotificationPublisher INotificationPublisherFacade.Client => Client;
    ILocalNoteNotificationPublisher INotificationPublisherFacade.Note => Note;
    ILocalNoteTypeNotificationPublisher INotificationPublisherFacade.NoteType => NoteType;
    ILocalSprintNotificationPublisher INotificationPublisherFacade.Sprint => Sprint;
    ILocalStatisticsDataNotificationPublisher INotificationPublisherFacade.StatisticsData => StatisticsData;
    ILocalTagNotificationPublisher INotificationPublisherFacade.Tag => Tag;
    ILocalTicketNotificationPublisher INotificationPublisherFacade.Ticket => Ticket;
    ILocalTimerSettingsNotificationPublisher INotificationPublisherFacade.TimerSettings => TimerSettings;
    ILocalWorkDayNotificationPublisher INotificationPublisherFacade.WorkDay => WorkDay;
}