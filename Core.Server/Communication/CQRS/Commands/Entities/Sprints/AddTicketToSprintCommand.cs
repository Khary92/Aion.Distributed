namespace Core.Server.Communication.CQRS.Commands.Entities.Sprints;

public record AddTicketToSprintCommand(Guid SprintId, Guid TicketId);