
namespace Domain.Events.Sprints;

public record SprintCreatedEvent(
    Guid SprintId,
    string Name,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    bool IsActive,
    List<Guid> TicketIds);