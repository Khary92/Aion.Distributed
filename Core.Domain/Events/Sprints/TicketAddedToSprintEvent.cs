
namespace Domain.Events.Sprints;

public record TicketAddedToSprintEvent(Guid SprintId, Guid TicketId);