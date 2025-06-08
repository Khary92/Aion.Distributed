using MediatR;

namespace Contract.CQRS.Commands.UseCase.Commands;

public record CreateTimeSlotControlCommand(Guid TicketId, Guid ViewId)
    : INotification, IRequest<Unit>;