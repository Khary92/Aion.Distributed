using MediatR;

namespace Contract.Notifications.Entities.Sprints;

public record TicketAddedToSprintNotification(Guid SprintId, Guid TicketId) : IRequest<Unit>, INotification;