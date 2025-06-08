using MediatR;

namespace Contract.CQRS.Commands.UseCase;

public record LoadTimeSlotControlCommand(Guid TimeSlotId, Guid ViewId) : IRequest<Unit>;