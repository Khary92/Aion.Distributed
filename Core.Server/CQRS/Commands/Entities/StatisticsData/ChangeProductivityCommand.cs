
namespace Application.Contract.CQRS.Commands.Entities.StatisticsData;

public record ChangeProductivityCommand(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive);