using Application.Contract.CQRS.Commands.Entities.NoteType;
using Application.Services.Entities.NoteTypes;
using MediatR;

namespace Application.Handler.Commands.Entities.NoteType;

public class ChangeNoteTypeNameCommandHandler(INoteTypeCommandsService noteTypeCommandsService)
    : IRequestHandler<ChangeNoteTypeNameCommand, Unit>
{
    public async Task<Unit> Handle(ChangeNoteTypeNameCommand command, CancellationToken cancellationToken)
    {
        await noteTypeCommandsService.ChangeName(command);
        return Unit.Value;
    }
}