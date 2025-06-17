namespace Domain.Events.StatisticsData;

public record TagSelectionChangedEvent(Guid TagId, List<Guid> SelectedTagIds);