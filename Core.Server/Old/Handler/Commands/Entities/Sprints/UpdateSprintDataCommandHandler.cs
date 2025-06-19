using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Old.Handler.Commands.Entities.Sprints;

public class UpdateSprintDataCommandHandler(ISprintCommandsService sprintCommandsService)
    : IRequestHandler<UpdateSprintDataCommand, Unit>
{
    public async Task<Unit> Handle(UpdateSprintDataCommand command, CancellationToken cancellationToken)
    {
        await sprintCommandsService.UpdateSprintData(command);
        return Unit.Value;
    }
}