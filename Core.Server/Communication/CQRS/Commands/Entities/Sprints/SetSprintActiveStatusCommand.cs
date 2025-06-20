namespace Core.Server.Communication.CQRS.Commands.Entities.Sprints;

public record SetSprintActiveStatusCommand(Guid SprintId, bool IsActive);