
namespace Application.Contract.CQRS.Commands.UseCase.Commands;

public record LoadTimeSlotControlCommand(Guid TimeSlotId, Guid ViewId);