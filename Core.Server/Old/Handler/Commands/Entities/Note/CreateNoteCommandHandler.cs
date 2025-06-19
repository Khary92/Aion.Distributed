using Service.Server.CQRS.Commands.Entities.Note;
using Service.Server.Old.Services.Entities.Notes;

namespace Service.Server.Old.Handler.Commands.Entities.Note;

public class CreateNoteCommandHandler(INoteCommandsService noteCommandsService)
    : IRequestHandler<CreateNoteCommand, Unit>
{
    public async Task<Unit> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        await noteCommandsService.Create(command);
        return Unit.Value;
    }
}