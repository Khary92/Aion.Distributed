namespace Domain.Events.Tags;

public record TagEvent(
    Guid EventId,
    DateTime TimeStamp,
    TimeSpan Offset,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;