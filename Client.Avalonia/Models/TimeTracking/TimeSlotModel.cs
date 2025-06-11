using System;
using System.Threading.Tasks;
using Client.Avalonia.Communication.RequiresChange.Cache;
using Client.Avalonia.Models.Synchronization;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.TimeSlots;
using Proto.Notifications.Ticket;
using Proto.Notifications.UseCase;
using ReactiveUI;

namespace Client.Avalonia.Models.TimeTracking;

public class TimeSlotModel(
    IMessenger messenger,
    IStateSynchronizer<TicketReplayDecorator, string> ticketDocumentStateSynchronizer,
    IPersistentCache<SetStartTimeCommand> startTimeCache,
    IPersistentCache<SetEndTimeCommand> endTimeCache) : ReactiveObject
{
    private TicketReplayDecorator _selectedTicketReplayDecorator = null!;
    private TimeSlotDto _timeSlot = null!;

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

    public TimeSlotDto TimeSlot
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
                var setEndTimeCommand = new SetEndTimeCommand
                {
                    TimeSlotId = TimeSlot.TimeSlotId.ToString(),
                    Time = Timestamp.FromDateTimeOffset(TimeSlot.EndTime)
                };
                endTimeCache.Store(setEndTimeCommand);
            }

            if (TimeSlot.IsStartTimeChanged())
            {
                var setStartTimeCommand = new SetStartTimeCommand
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