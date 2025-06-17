namespace Domain.Events.StatisticsData;

public record ProductivityChangedEvent(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive);