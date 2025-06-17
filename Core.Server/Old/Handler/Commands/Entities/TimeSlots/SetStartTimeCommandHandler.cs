using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Commands.Entities.TimeSlots;

public class SetStartTimeCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<SetStartTimeCommand, Unit>
{
    public async Task<Unit> Handle(SetStartTimeCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.SetStartTime(command);
        return Unit.Value;
    }
}