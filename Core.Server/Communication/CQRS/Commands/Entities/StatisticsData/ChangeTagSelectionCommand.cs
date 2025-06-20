namespace Core.Server.Communication.CQRS.Commands.Entities.StatisticsData;

public record ChangeTagSelectionCommand(Guid StatisticsDataId, List<Guid> SelectedTagIds);