namespace Domain.Events.Sprints;

public record SprintDataUpdatedEvent(
    Guid SprintId,
    string Name,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);