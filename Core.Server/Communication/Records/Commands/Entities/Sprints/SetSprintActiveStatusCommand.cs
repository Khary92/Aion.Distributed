namespace Core.Server.Communication.Records.Commands.Entities.Sprints;

public record SetSprintActiveStatusCommand(Guid SprintId, bool IsActive);