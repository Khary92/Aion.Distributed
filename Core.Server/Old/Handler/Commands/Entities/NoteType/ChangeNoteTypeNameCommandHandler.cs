using Service.Server.CQRS.Commands.Entities.NoteType;
using Service.Server.Old.Services.Entities.NoteTypes;

namespace Service.Server.Old.Handler.Commands.Entities.NoteType;

public class ChangeNoteTypeNameCommandHandler(INoteTypeCommandsService noteTypeCommandsService)
    : IRequestHandler<ChangeNoteTypeNameCommand, Unit>
{
    public async Task<Unit> Handle(ChangeNoteTypeNameCommand command, CancellationToken cancellationToken)
    {
        await noteTypeCommandsService.ChangeName(command);
        return Unit.Value;
    }
}