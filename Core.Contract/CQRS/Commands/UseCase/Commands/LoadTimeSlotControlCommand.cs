using MediatR;

namespace Contract.CQRS.Commands.UseCase.Commands;

public record LoadTimeSlotControlCommand(Guid TimeSlotId, Guid ViewId) : IRequest<Unit>;