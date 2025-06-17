namespace Domain.Events.NoteTypes;

public record NoteTypeEvent(
    Guid EventId,
    DateTime TimeStamp,
    TimeSpan Offset,
    string EventType,
    Guid EntityId,
    string EventPayload) : IDomainEvent;