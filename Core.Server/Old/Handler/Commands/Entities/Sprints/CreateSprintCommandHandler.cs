using Application.Contract.CQRS.Commands.Entities.Sprints;
using Application.Services.Entities.Sprints;
using MediatR;

namespace Application.Handler.Commands.Entities.Sprints;

public class CreateSprintCommandHandler(ISprintCommandsService sprintCommandsService)
    : IRequestHandler<CreateSprintCommand, Unit>
{
    public async Task<Unit> Handle(CreateSprintCommand command, CancellationToken cancellationToken)
    {
        await sprintCommandsService.Create(command);
        return Unit.Value;
    }
}