using System.Threading.Tasks;
using Client.Avalonia.Communication.RequiresChange.Cache;
using Client.Avalonia.ViewModels.Synchronization;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Tickets;
using Contract.CQRS.Notifications.UseCase;
using Contract.DTO;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.TimeTracking;

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
            if (TicketReplayDecorator.Ticket.TicketId == m.TicketId) return;

            TicketReplayDecorator.Ticket.Apply(m);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            if (TicketReplayDecorator.Ticket.TicketId == m.TicketId) return;

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
                endTimeCache.Store(new SetEndTimeCommand(TimeSlot.TimeSlotId, TimeSlot.EndTime));

            if (TimeSlot.IsStartTimeChanged())
                startTimeCache.Store(new SetStartTimeCommand(TimeSlot.TimeSlotId, TimeSlot.StartTime));
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