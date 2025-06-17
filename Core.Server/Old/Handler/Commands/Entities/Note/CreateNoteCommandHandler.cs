using Application.Contract.CQRS.Commands.Entities.Note;
using Application.Services.Entities.Notes;
using MediatR;

namespace Application.Handler.Commands.Entities.Note;

public class CreateNoteCommandHandler(INoteCommandsService noteCommandsService)
    : IRequestHandler<CreateNoteCommand, Unit>
{
    public async Task<Unit> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        await noteCommandsService.Create(command);
        return Unit.Value;
    }
}