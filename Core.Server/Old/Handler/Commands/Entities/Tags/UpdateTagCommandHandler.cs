using Service.Server.CQRS.Commands.Entities.Tags;
using Service.Server.Old.Services.Entities.Tags;

namespace Service.Server.Old.Handler.Commands.Entities.Tags;

public class UpdateTagCommandHandler(ITagCommandsService tagCommandsService)
    : IRequestHandler<UpdateTagCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        await tagCommandsService.Update(command);
        return Unit.Value;
    }
}