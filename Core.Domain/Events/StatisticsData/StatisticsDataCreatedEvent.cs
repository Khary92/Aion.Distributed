namespace Domain.Events.StatisticsData;

public record StatisticsDataCreatedEvent(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive,
    List<Guid> TagIds,
    Guid TimeSlotId);