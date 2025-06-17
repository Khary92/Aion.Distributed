using Application.Contract.CQRS.Commands.Entities.Tags;
using Application.Services.Entities.Tags;
using MediatR;

namespace Application.Handler.Commands.Entities.Tags;

public class CreateTagCommandHandler(ITagCommandsService tagCommandsService)
    : IRequestHandler<CreateTagCommand, Unit>
{
    public async Task<Unit> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        await tagCommandsService.Create(command);
        return Unit.Value;
    }
}