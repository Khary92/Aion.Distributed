
namespace Service.Server.CQRS.Commands.UseCase.Commands;

public record LoadTimeSlotControlCommand(Guid TimeSlotId, Guid ViewId);