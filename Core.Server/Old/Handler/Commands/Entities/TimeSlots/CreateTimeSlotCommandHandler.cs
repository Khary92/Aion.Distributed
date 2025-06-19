using Service.Server.CQRS.Commands.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Commands.Entities.TimeSlots;

public class CreateTimeSlotCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<CreateTimeSlotCommand, Unit>
{
    public async Task<Unit> Handle(CreateTimeSlotCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.Create(command);
        return Unit.Value;
    }
}