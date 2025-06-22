namespace Core.Server.Communication.Records.Commands.Entities.Sprints;

public record AddTicketToSprintCommand(Guid SprintId, Guid TicketId);