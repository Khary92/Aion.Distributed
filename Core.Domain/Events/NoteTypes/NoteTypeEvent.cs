namespace Domain.Events.NoteTypes;

public record NoteTypeEvent(
    Guid EventId,
    DateTimeOffset TimeStamp,
    string EventType,
    Guid EntityId,
    string EventPayload) : IDomainEvent;