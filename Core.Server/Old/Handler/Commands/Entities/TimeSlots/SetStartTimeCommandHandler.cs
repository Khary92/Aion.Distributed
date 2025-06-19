using Service.Server.CQRS.Commands.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Commands.Entities.TimeSlots;

public class SetStartTimeCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<SetStartTimeCommand, Unit>
{
    public async Task<Unit> Handle(SetStartTimeCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.SetStartTime(command);
        return Unit.Value;
    }
}