using Application.Contract.CQRS.Commands.Entities.Sprints;
using Application.Services.Entities.Sprints;
using MediatR;

namespace Application.Handler.Commands.Entities.Sprints;

public class SetSprintActiveStatusCommandHandler(
    ISprintCommandsService sprintCommandsService,
    ISprintRequestsService sprintRequestsService) : IRequestHandler<SetSprintActiveStatusCommand, Unit>
{
    public async Task<Unit> Handle(SetSprintActiveStatusCommand command, CancellationToken cancellationToken)
    {
        var sprintDtos = await sprintRequestsService.GetAll();

        foreach (var sprint in sprintDtos.Where(sprint => sprint.IsActive))
            await sprintCommandsService.SetSprintActiveStatus(new SetSprintActiveStatusCommand(sprint.SprintId, false));

        await sprintCommandsService.SetSprintActiveStatus(command);
        return Unit.Value;
    }
}