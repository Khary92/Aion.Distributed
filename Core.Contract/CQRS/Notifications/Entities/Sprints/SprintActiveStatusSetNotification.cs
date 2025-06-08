using MediatR;

namespace Contract.CQRS.Notifications.Entities.Sprints;

public record SprintActiveStatusSetNotification(Guid SprintId, bool IsActive)
    : IRequest<Unit>, INotification;