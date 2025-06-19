using Service.Server.CQRS.Commands.Entities.Note;
using Service.Server.Old.Services.Entities.Notes;

namespace Service.Server.Old.Handler.Commands.Entities.Note;

public class UpdateNoteCommandHandler(INoteCommandsService noteCommandsService)
    : IRequestHandler<UpdateNoteCommand, Unit>
{
    public async Task<Unit> Handle(UpdateNoteCommand command, CancellationToken cancellationToken)
    {
        await noteCommandsService.Update(command);
        return Unit.Value;
    }
}