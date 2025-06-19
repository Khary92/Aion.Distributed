using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Old.Handler.Commands.Entities.Sprints;

public class CreateSprintCommandHandler(ISprintCommandsService sprintCommandsService)
    : IRequestHandler<CreateSprintCommand, Unit>
{
    public async Task<Unit> Handle(CreateSprintCommand command, CancellationToken cancellationToken)
    {
        await sprintCommandsService.Create(command);
        return Unit.Value;
    }
}