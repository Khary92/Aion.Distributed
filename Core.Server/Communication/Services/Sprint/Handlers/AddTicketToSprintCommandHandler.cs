using Service.Server.Communication.CQRS.Commands.Entities.Sprints;
using Service.Server.Services.Entities.Sprints;

namespace Service.Server.Communication.Services.Sprint.Handlers;

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