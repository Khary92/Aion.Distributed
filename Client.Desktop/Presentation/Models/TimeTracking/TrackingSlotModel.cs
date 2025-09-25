using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Services.Cache;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TrackingSlotModel(
    IMessenger messenger,
    IPersistentCache<ClientSetStartTimeCommand> startTimeCache,
    IPersistentCache<ClientSetEndTimeCommand> endTimeCache,
    ITraceCollector tracer) : ReactiveObject, IRecipient<ClientTicketDocumentationUpdatedNotification>,
    IRecipient<ClientTicketDataUpdatedNotification>,
    IRecipient<ClientCreateSnapshotNotification>
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
        messenger.RegisterAll(this);
    }

    public void ToggleTimerState()
    {
        TimeSlot.IsTimerRunning = !TimeSlot.IsTimerRunning;
    }

    public async Task ToggleReplayMode()
    {
//TODO
    }

    public void Receive(ClientTicketDocumentationUpdatedNotification message)
    {
        if (Ticket.TicketId != message.TicketId) return;

        Ticket.Apply(message);
        _ = tracer.Ticket.ChangeDocumentation.NotificationReceived(GetType(), message.TraceId, message);
    }

    public void Receive(ClientTicketDataUpdatedNotification message)
    {
        if (Ticket.TicketId != message.TicketId) return;

        Ticket.Apply(message);
        _ = tracer.Ticket.Update.ChangesApplied(GetType(), message.TraceId);
    }

    public void Receive(ClientCreateSnapshotNotification message)
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
    }
}