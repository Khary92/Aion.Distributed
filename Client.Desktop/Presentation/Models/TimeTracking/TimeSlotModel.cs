using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Services.Cache;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeSlotModel(
    IMessenger messenger,
    IStateSynchronizer<TicketReplayDecorator, string> ticketDocumentStateSynchronizer,
    IPersistentCache<ClientSetStartTimeCommand> startTimeCache,
    IPersistentCache<ClientSetEndTimeCommand> endTimeCache,
    ITraceCollector tracer) : ReactiveObject
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
        messenger.Register<ClientTicketDocumentationUpdatedNotification>(this, (_, notification) =>
        {
            if (TicketReplayDecorator.Ticket.TicketId == notification.TicketId) return;

            TicketReplayDecorator.Ticket.Apply(notification);
        });

        messenger.Register<ClientTicketDataUpdatedNotification>(this, (_, notification) =>
        {
            if (TicketReplayDecorator.Ticket.TicketId == notification.TicketId) return;

            TicketReplayDecorator.Ticket.Apply(notification);
        });

        messenger.Register<ClientSaveDocumentationNotification>(this, async void (_, _) =>
        {
            if (!TicketReplayDecorator.Ticket.IsDocumentationChanged()) return;

            var traceId = Guid.NewGuid();
            await tracer.Ticket.Documentation.StartUseCase(GetType(),traceId);
            
            ticketDocumentStateSynchronizer.SetSynchronizationValue(TicketReplayDecorator.Ticket.TicketId,
                TicketReplayDecorator.Ticket.Documentation);

            await ticketDocumentStateSynchronizer.FireCommand(traceId);
        });

        messenger.Register<ClientCreateSnapshotNotification>(this, (_, _) =>
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
        });
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
}