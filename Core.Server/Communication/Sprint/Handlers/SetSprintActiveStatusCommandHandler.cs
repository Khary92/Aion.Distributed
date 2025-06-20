using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Old.Handler.Commands.Entities.Sprints;

public class SetSprintActiveStatusCommandHandler(
    ISprintCommandsService sprintCommandsService,
    ISprintRequestsService sprintRequestsService)
{
    public async Task Handle(SetSprintActiveStatusCommand command)
    {
        var sprintDtos = await sprintRequestsService.GetAll();

        foreach (var sprint in sprintDtos.Where(sprint => sprint.IsActive))
            await sprintCommandsService.SetSprintActiveStatus(new SetSprintActiveStatusCommand(sprint.SprintId, false));

        await sprintCommandsService.SetSprintActiveStatus(command);
    }
}