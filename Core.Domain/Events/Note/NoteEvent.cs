namespace Domain.Events.Note;

public record NoteEvent(
    Guid EventId,
    DateTime TimeStamp,
    TimeSpan Offset,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;