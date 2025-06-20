
namespace Service.Server.Communication.CQRS.Commands.Entities.Sprints;

public record UpdateSprintDataCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);