using Application.Contract.CQRS.Commands.Entities.NoteType;
using Application.Services.Entities.NoteTypes;
using MediatR;

namespace Application.Handler.Commands.Entities.NoteType;

public class CreateNoteTypeCommandHandler(INoteTypeCommandsService noteTypeCommandsService)
    : IRequestHandler<CreateNoteTypeCommand, Unit>
{
    public async Task<Unit> Handle(CreateNoteTypeCommand command, CancellationToken cancellationToken)
    {
        await noteTypeCommandsService.Create(command);
        return Unit.Value;
    }
}