
namespace Service.Server.Communication.CQRS.Commands.Entities.StatisticsData;

public record CreateStatisticsDataCommand(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive,
    List<Guid> TagIds,
    Guid TimeSlotId);