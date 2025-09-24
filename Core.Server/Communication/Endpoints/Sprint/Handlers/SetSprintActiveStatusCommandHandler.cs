using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Services.Entities.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint.Handlers;

public class SetSprintActiveStatusCommandHandler(
    ISprintCommandsService sprintCommandsService,
    ISprintRequestsService sprintRequestsService) : ISetSprintActiveStatusCommandHandler
{
    public async Task Handle(SetSprintActiveStatusCommand command)
    {
        var sprintDtos = await sprintRequestsService.GetAll();

        foreach (var sprint in sprintDtos.Where(sprint => sprint.IsActive))
            await sprintCommandsService.SetSprintActiveStatus(
                new SetSprintActiveStatusCommand(sprint.SprintId, false, command.TraceId));

        await sprintCommandsService.SetSprintActiveStatus(command);
    }
}