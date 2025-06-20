namespace Core.Server.Communication.CQRS.Commands.Entities.Sprints;

public record AddTicketToActiveSprintCommand(Guid TicketId);