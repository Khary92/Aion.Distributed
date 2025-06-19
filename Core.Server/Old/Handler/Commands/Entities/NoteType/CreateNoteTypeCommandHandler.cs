using Service.Server.CQRS.Commands.Entities.NoteType;
using Service.Server.Old.Services.Entities.NoteTypes;

namespace Service.Server.Old.Handler.Commands.Entities.NoteType;

public class CreateNoteTypeCommandHandler(INoteTypeCommandsService noteTypeCommandsService)
    : IRequestHandler<CreateNoteTypeCommand, Unit>
{
    public async Task<Unit> Handle(CreateNoteTypeCommand command, CancellationToken cancellationToken)
    {
        await noteTypeCommandsService.Create(command);
        return Unit.Value;
    }
}