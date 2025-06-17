
namespace Domain.Events.Sprints;

public record SprintActiveStatusChangedEvent(Guid SprintId, bool IsActive);