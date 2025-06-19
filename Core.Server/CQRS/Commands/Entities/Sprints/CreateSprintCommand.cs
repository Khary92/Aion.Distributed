
namespace Service.Server.CQRS.Commands.Entities.Sprints;

public record CreateSprintCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    bool IsActive,
    List<Guid> TicketIds);