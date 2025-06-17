namespace Domain.Events.Sprints;

public record SprintEvent(
    Guid EventId,
    DateTime TimeStamp,
    TimeSpan Offset,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;