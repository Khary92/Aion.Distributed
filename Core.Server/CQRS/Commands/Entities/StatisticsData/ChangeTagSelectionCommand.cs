
namespace Service.Server.CQRS.Commands.Entities.StatisticsData;

public record ChangeTagSelectionCommand(Guid StatisticsDataId, List<Guid> SelectedTagIds);