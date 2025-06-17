using Application.Contract.CQRS.Commands.Entities.NoteType;
using Application.Services.Entities.NoteTypes;
using MediatR;

namespace Application.Handler.Commands.Entities.NoteType;

public class ChangeNoteTypeColorCommandHandler(INoteTypeCommandsService noteTypeCommandsService)
    : IRequestHandler<ChangeNoteTypeColorCommand, Unit>
{
    public async Task<Unit> Handle(ChangeNoteTypeColorCommand command, CancellationToken cancellationToken)
    {
        await noteTypeCommandsService.ChangeColor(command);
        return Unit.Value;
    }
}