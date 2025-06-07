using MediatR;

namespace Contract.Notifications.Entities.Sprints;

public record SprintActiveStatusSetNotification(Guid SprintId, bool IsActive)
    : IRequest<Unit>, INotification;