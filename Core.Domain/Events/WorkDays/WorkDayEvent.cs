namespace Domain.Events.WorkDays;

public record WorkDayEvent(
    Guid EventId,
    DateTimeOffset TimeStamp,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;