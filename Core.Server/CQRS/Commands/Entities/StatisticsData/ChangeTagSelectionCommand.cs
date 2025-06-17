
namespace Application.Contract.CQRS.Commands.Entities.StatisticsData;

public record ChangeTagSelectionCommand(Guid StatisticsDataId, List<Guid> SelectedTagIds);