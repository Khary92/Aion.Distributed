using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Services.Cache;
using CommunityToolkit.Mvvm.Messaging;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.TimeSlots;
using Proto.Notifications.Ticket;
using Proto.Notifications.UseCase;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeSlotModel(
    IMessenger messenger,
    IStateSynchronizer<TicketReplayDecorator, string> ticketDocumentStateSynchronizer,
    IPersistentCache<SetStartTimeCommandProto> startTimeCache,
    IPersistentCache<SetEndTimeCommandProto> endTimeCache) : ReactiveObject
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
        messenger.Register<TicketDocumentationUpdatedNotification>(this, (_, m) =>
        {
            if (TicketReplayDecorator.Ticket.TicketId == Guid.Parse(m.TicketId)) return;

            TicketReplayDecorator.Ticket.Apply(m);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            if (TicketReplayDecorator.Ticket.TicketId == Guid.Parse(m.TicketId)) return;

            TicketReplayDecorator.Ticket.Apply(m);
        });

        messenger.Register<SaveDocumentationNotification>(this, async void (_, _) =>
        {
            if (!TicketReplayDecorator.Ticket.IsDocumentationChanged()) return;

            ticketDocumentStateSynchronizer.SetSynchronizationValue(TicketReplayDecorator.Ticket.TicketId,
                TicketReplayDecorator.Ticket.Documentation);

            await ticketDocumentStateSynchronizer.FireCommand();
        });

        messenger.Register<CreateSnapshotNotification>(this, (_, _) =>
        {
            if (TimeSlot.IsEndTimeChanged())
            {
                var setEndTimeCommand = new SetEndTimeCommandProto
                {
                    TimeSlotId = TimeSlot.TimeSlotId.ToString(),
                    Time = Timestamp.FromDateTimeOffset(TimeSlot.EndTime)
                };
                endTimeCache.Store(setEndTimeCommand);
            }

            if (TimeSlot.IsStartTimeChanged())
            {
                var setStartTimeCommand = new SetStartTimeCommandProto
                {
                    TimeSlotId = TimeSlot.TimeSlotId.ToString(),
                    Time = Timestamp.FromDateTimeOffset(TimeSlot.StartTime)
                };

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