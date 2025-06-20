using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Communication.Sprint.Handlers;

public class AddTicketToSprintCommandHandler(
    ISprintRequestsService sprintRequestsService,
    ISprintCommandsService sprintCommandsService)
{
    public async Task Handle(AddTicketToSprintCommand command)
    {
        var activeSprint = await sprintRequestsService.GetById(command.SprintId);

        if (activeSprint == null) return;

        await sprintCommandsService.AddTicketToSprint(command);
    }
}