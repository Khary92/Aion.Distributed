using Service.Server.CQRS.Commands.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Commands.Entities.TimeSlots;

public class SetEndTimeCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<SetEndTimeCommand, Unit>
{
    public async Task<Unit> Handle(SetEndTimeCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.SetEndTime(command);
        return Unit.Value;
    }
}