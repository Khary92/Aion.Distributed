using MediatR;

namespace Contract.CQRS.Commands.UseCase;

public record CreateTimeSlotControlCommand(Guid TicketId, Guid ViewId)
    : INotification, IRequest<Unit>;