namespace Domain.Events.Note;

public record NoteEvent(
    Guid EventId,
    DateTimeOffset TimeStamp,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;