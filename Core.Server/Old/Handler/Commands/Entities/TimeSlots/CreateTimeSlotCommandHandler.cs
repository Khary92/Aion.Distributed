using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Commands.Entities.TimeSlots;

public class CreateTimeSlotCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<CreateTimeSlotCommand, Unit>
{
    public async Task<Unit> Handle(CreateTimeSlotCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.Create(command);
        return Unit.Value;
    }
}