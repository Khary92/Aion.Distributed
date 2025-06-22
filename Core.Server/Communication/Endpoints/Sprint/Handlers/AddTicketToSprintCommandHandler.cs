using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Services.Entities.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint.Handlers;

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