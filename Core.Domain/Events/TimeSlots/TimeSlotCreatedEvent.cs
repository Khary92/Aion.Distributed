namespace Domain.Events.TimeSlots;

public record TimeSlotCreatedEvent(
    Guid TimeSlotId,
    Guid SelectedTicketId,
    Guid WorkDayId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    bool IsTimerRunning,
    List<Guid> NoteIds);