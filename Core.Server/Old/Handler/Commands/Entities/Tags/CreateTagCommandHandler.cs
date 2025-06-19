using Service.Server.CQRS.Commands.Entities.Tags;
using Service.Server.Old.Services.Entities.Tags;

namespace Service.Server.Old.Handler.Commands.Entities.Tags;

public class CreateTagCommandHandler(ITagCommandsService tagCommandsService)
    : IRequestHandler<CreateTagCommand, Unit>
{
    public async Task<Unit> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        await tagCommandsService.Create(command);
        return Unit.Value;
    }
}