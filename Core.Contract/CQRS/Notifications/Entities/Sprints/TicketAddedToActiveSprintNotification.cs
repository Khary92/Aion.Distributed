using MediatR;

namespace Contract.CQRS.Notifications.Entities.Sprints;

public record TicketAddedToActiveSprintNotification : IRequest<Unit>, INotification;