using MediatR;

namespace Contract.CQRS.Notifications.Entities.Sprints;

public record TicketAddedToSprintNotification(Guid SprintId, Guid TicketId) : IRequest<Unit>, INotification;