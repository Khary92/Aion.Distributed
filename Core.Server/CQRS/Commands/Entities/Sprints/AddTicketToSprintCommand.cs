
namespace Service.Server.CQRS.Commands.Entities.Sprints;

public record AddTicketToSprintCommand(Guid SprintId, Guid TicketId);