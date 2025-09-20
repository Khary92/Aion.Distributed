namespace Domain.Events.StatisticsData;

public record TagSelectionChangedEvent(Guid StatisticsId, List<Guid> SelectedTagIds);