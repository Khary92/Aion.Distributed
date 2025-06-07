using MediatR;

namespace Contract.Notifications.Entities.Sprints;

public record TicketAddedToActiveSprintNotification : IRequest<Unit>, INotification;