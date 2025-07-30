namespace Core.Server.Communication.Records.Commands.Entities.StatisticsData;

public record ChangeProductivityCommand(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive,
    Guid TraceId);