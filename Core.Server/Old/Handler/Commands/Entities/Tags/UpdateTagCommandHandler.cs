using Application.Contract.CQRS.Commands.Entities.Tags;
using Application.Services.Entities.Tags;
using MediatR;

namespace Application.Handler.Commands.Entities.Tags;

public class UpdateTagCommandHandler(ITagCommandsService tagCommandsService)
    : IRequestHandler<UpdateTagCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        await tagCommandsService.Update(command);
        return Unit.Value;
    }
}