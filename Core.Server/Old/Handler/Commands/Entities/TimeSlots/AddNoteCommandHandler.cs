using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Commands.Entities.TimeSlots;

public class AddNoteCommandHandler(ITimeSlotCommandsService timeSlotCommandsService)
    : IRequestHandler<AddNoteCommand, Unit>
{
    public async Task<Unit> Handle(AddNoteCommand command, CancellationToken cancellationToken)
    {
        await timeSlotCommandsService.AddNote(command);
        return Unit.Value;
    }
}