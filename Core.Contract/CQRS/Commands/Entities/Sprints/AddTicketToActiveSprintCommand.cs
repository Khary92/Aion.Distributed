using MediatR;

namespace Contract.CQRS.Commands.Entities.Sprints;

public record AddTicketToActiveSprintCommand(Guid TicketId) : IRequest<Unit>, INotification;