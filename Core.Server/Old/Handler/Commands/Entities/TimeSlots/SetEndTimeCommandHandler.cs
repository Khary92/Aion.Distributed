using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Commands.Entities.TimeSlots;

public class SetEndTimeCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<SetEndTimeCommand, Unit>
{
    public async Task<Unit> Handle(SetEndTimeCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.SetEndTime(command);
        return Unit.Value;
    }
}