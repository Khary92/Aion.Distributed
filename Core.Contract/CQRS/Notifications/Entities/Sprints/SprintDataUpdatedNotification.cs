using MediatR;

namespace Contract.CQRS.Notifications.Entities.Sprints;

public record SprintDataUpdatedNotification(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime)
    : IRequest<Unit>, INotification;