using Service.Server.CQRS.Commands.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Commands.Entities.TimeSlots;

public class AddNoteCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<AddNoteCommand, Unit>
{
    public async Task<Unit> Handle(AddNoteCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.AddNote(command);
        return Unit.Value;
    }
}