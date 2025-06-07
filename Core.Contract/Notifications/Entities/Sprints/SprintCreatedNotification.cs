using MediatR;

namespace Contract.Notifications.Entities.Sprints;

public record SprintCreatedNotification(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    bool IsActive,
    List<Guid> TicketIds)
    : IRequest<Unit>, INotification;