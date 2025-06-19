using Service.Server.CQRS.Commands.Entities.NoteType;
using Service.Server.Old.Services.Entities.NoteTypes;

namespace Service.Server.Old.Handler.Commands.Entities.NoteType;

public class ChangeNoteTypeColorCommandHandler(INoteTypeCommandsService noteTypeCommandsService)
    : IRequestHandler<ChangeNoteTypeColorCommand, Unit>
{
    public async Task<Unit> Handle(ChangeNoteTypeColorCommand command, CancellationToken cancellationToken)
    {
        await noteTypeCommandsService.ChangeColor(command);
        return Unit.Value;
    }
}