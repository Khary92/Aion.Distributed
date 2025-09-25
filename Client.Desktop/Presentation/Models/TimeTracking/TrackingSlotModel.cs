using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Services.Cache;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TrackingSlotModel(
    IMessenger messenger,
    IStateSynchronizer<TicketReplayDecorator, string> ticketDocumentStateSynchronizer,
    IPersistentCache<ClientSetStartTimeCommand> startTimeCache,
    IPersistentCache<ClientSetEndTimeCommand> endTimeCache,
    ITraceCollector tracer) : ReactiveObject, IRecipient<ClientTicketDocumentationUpdatedNotification>,
    IRecipient<ClientTicketDataUpdatedNotification>, IRecipient<ClientSaveDocumentationNotification>,
    IRecipient<ClientCreateSnapshotNotification>
{
    private TicketReplayDecorator _selectedTicketReplayDecorator = null!;
    private TimeSlotClientModel _timeSlot = null!;

    public TicketReplayDecorator TicketReplayDecorator
    {
        get => _selectedTicketReplayDecorator;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTicketReplayDecorator, value);

            ticketDocumentStateSynchronizer.Register(_selectedTicketReplayDecorator.Ticket.TicketId,
                _selectedTicketReplayDecorator);
        }
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
        TicketReplayDecorator.IsReplayMode = !TicketReplayDecorator.IsReplayMode;

        if (TicketReplayDecorator.IsReplayMode) await TicketReplayDecorator.LoadHistory();

        if (!TicketReplayDecorator.IsReplayMode) TicketReplayDecorator.ExitReplay();
    }

    public void Receive(ClientTicketDocumentationUpdatedNotification message)
    {
        if (TicketReplayDecorator.Ticket.TicketId != message.TicketId) return;

        TicketReplayDecorator.Ticket.Apply(message);
        _ = tracer.Ticket.Documentation.NotificationReceived(GetType(), message.TraceId, message);
    }

    public void Receive(ClientTicketDataUpdatedNotification message)
    {
        if (TicketReplayDecorator.Ticket.TicketId != message.TicketId) return;

        TicketReplayDecorator.Ticket.Apply(message);
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
    
    public void Receive(ClientSaveDocumentationNotification message) => _ = HandleClientSaveDocumentationNotification(message);
    private async Task HandleClientSaveDocumentationNotification(ClientSaveDocumentationNotification message)
    {
        if (!TicketReplayDecorator.Ticket.IsDocumentationChanged()) return;

        var traceId = Guid.NewGuid();
        await tracer.Ticket.Documentation.StartUseCase(GetType(), traceId);

        ticketDocumentStateSynchronizer.SetSynchronizationValue(TicketReplayDecorator.Ticket.TicketId,
            TicketReplayDecorator.Ticket.Documentation);

        await ticketDocumentStateSynchronizer.FireCommand(traceId);
    }
}