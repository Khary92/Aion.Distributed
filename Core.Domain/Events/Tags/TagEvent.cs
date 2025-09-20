namespace Domain.Events.Tags;

public record TagEvent(
    Guid EventId,
    DateTimeOffset TimeStamp,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;