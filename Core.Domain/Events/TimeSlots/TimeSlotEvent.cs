namespace Domain.Events.TimeSlots;

public record TimeSlotEvent(
    Guid EventId,
    DateTime TimeStamp,
    TimeSpan Offset,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;