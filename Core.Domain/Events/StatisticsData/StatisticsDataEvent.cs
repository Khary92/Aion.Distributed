namespace Domain.Events.StatisticsData;

public record StatisticsDataEvent(
    Guid EventId,
    DateTimeOffset TimeStamp,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;