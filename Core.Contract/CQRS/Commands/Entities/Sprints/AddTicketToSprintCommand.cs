using MediatR;

namespace Contract.CQRS.Commands.Entities.Sprints;

public record AddTicketToSprintCommand(Guid SprintId, Guid TicketId) : IRequest<Unit>, INotification;