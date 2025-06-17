namespace Domain.Events;

public interface IDomainEvent
{
    Guid EventId { get; init; }
    DateTime TimeStamp { get; init; }
    TimeSpan Offset { get; init; }
    string EventType { get; init; }
    Guid EntityId { get; init; }
    string EventPayload { get; init; }
}