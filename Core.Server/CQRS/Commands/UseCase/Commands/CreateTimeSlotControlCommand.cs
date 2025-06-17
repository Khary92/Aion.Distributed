
namespace Application.Contract.CQRS.Commands.UseCase.Commands;

public record CreateTimeSlotControlCommand(Guid TicketId, Guid ViewId);