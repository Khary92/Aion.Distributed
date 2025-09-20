namespace Domain.Events;

public interface IDomainEvent
{
    Guid EventId { get; init; }
    DateTimeOffset TimeStamp { get; init; }
    string EventType { get; init; }
    Guid EntityId { get; init; }
    string EventPayload { get; init; }
}