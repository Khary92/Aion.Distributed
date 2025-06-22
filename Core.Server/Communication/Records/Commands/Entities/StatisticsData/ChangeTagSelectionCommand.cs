namespace Core.Server.Communication.Records.Commands.Entities.StatisticsData;

public record ChangeTagSelectionCommand(Guid StatisticsDataId, List<Guid> SelectedTagIds);