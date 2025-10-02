using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.Cache;
using Client.Tracing.Tracing.Tracers;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TrackingSlotModel(
    IPersistentCache<ClientSetStartTimeCommand> startTimeCache,
    IPersistentCache<ClientSetEndTimeCommand> endTimeCache,
    ITraceCollector tracer,
    INotificationPublisherFacade notificationPublisher,
    IClientTimerNotificationPublisher timerNotificationPublisher) : ReactiveObject, IMessengerRegistration
{
    private TicketClientModel _ticket = null!;
    private TimeSlotClientModel _timeSlot = null!;

    public TicketClientModel Ticket
    {
        get => _ticket;
        set => this.RaiseAndSetIfChanged(ref _ticket, value);
    }

    public TimeSlotClientModel TimeSlot
    {
        get => _timeSlot;
        set => this.RaiseAndSetIfChanged(ref _timeSlot, value);
    }

    public void RegisterMessenger()
    {
        notificationPublisher.Ticket.TicketDocumentationUpdatedNotificationReceived +=
            HandleClientTicketDocumentationUpdatedNotification;
        notificationPublisher.Ticket.TicketDataUpdatedNotificationReceived += HandleClientTicketDataUpdatedNotification;
        timerNotificationPublisher.ClientCreateSnapshotNotificationReceived += HandleClientCreateSnapshotNotification;
    }

    public void UnregisterMessenger()
    {
        notificationPublisher.Ticket.TicketDocumentationUpdatedNotificationReceived -=
            HandleClientTicketDocumentationUpdatedNotification;
        notificationPublisher.Ticket.TicketDataUpdatedNotificationReceived -= HandleClientTicketDataUpdatedNotification;
        timerNotificationPublisher.ClientCreateSnapshotNotificationReceived -= HandleClientCreateSnapshotNotification;
    }

    private Task HandleClientCreateSnapshotNotification(ClientCreateSnapshotNotification message)
    {
        if (TimeSlot.IsEndTimeChanged())
        {
            var setEndTimeCommand =
                new ClientSetEndTimeCommand(TimeSlot.TimeSlotId, TimeSlot.EndTime, Guid.NewGuid());

            endTimeCache.Store(setEndTimeCommand);
        }

        if (TimeSlot.IsStartTimeChanged())
        {
            var setStartTimeCommand =
                new ClientSetStartTimeCommand(TimeSlot.TimeSlotId, TimeSlot.StartTime, Guid.NewGuid());

            startTimeCache.Store(setStartTimeCommand);
        }

        return Task.CompletedTask;
    }

    private async Task HandleClientTicketDataUpdatedNotification(ClientTicketDataUpdatedNotification message)
    {
        if (Ticket.TicketId != message.TicketId) return;

        Ticket.Apply(message);
        await tracer.Ticket.Update.ChangesApplied(GetType(), message.TraceId);
    }

    private async Task HandleClientTicketDocumentationUpdatedNotification(
        ClientTicketDocumentationUpdatedNotification message)
    {
        if (Ticket.TicketId != message.TicketId) return;

        Ticket.Apply(message);
        await tracer.Ticket.ChangeDocumentation.NotificationReceived(GetType(), message.TraceId, message);
    }

    public void ToggleTimerState()
    {
        TimeSlot.IsTimerRunning = !TimeSlot.IsTimerRunning;
    }

    public async Task ToggleReplayMode()
    {
        Ticket.IsReplayMode = !Ticket.IsReplayMode;

        if (!Ticket.IsReplayMode) return;

        await Ticket!.TicketReplayProvider!.LoadHistory(_ticket.TicketId);
    }
}