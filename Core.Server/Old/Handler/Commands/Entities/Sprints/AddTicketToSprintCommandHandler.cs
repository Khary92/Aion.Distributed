using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Old.Handler.Commands.Entities.Sprints;

public class AddTicketToSprintCommandHandler(
    ISprintRequestsService sprintRequestsService,
    ISprintCommandsService sprintCommandsService)
    : IRequestHandler<AddTicketToSprintCommand, Unit>
{
    public async Task<Unit> Handle(AddTicketToSprintCommand command, CancellationToken cancellationToken)
    {
        var activeSprint = await sprintRequestsService.GetById(command.SprintId);

        if (activeSprint == null) return Unit.Value;

        await sprintCommandsService.AddTicketToSprint(command);
        return Unit.Value;
    }
}