using Application.Contract.CQRS.Commands.Entities.Sprints;
using Application.Services.Entities.Sprints;
using MediatR;

namespace Application.Handler.Commands.Entities.Sprints;

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