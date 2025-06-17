using Application.Contract.CQRS.Commands.Entities.Note;
using Application.Services.Entities.Notes;
using MediatR;

namespace Application.Handler.Commands.Entities.Note;

public class UpdateNoteCommandHandler(INoteCommandsService noteCommandsService)
    : IRequestHandler<UpdateNoteCommand, Unit>
{
    public async Task<Unit> Handle(UpdateNoteCommand command, CancellationToken cancellationToken)
    {
        await noteCommandsService.Update(command);
        return Unit.Value;
    }
}