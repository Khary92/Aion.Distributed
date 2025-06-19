
namespace Service.Server.CQRS.Commands.Entities.Sprints;

public record SetSprintActiveStatusCommand(Guid SprintId, bool IsActive);