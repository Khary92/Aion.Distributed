using Application.Contract.CQRS.Commands.Entities.Sprints;
using Application.Services.Entities.Sprints;
using MediatR;

namespace Application.Handler.Commands.Entities.Sprints;

public class UpdateSprintDataCommandHandler(ISprintCommandsService sprintCommandsService)
    : IRequestHandler<UpdateSprintDataCommand, Unit>
{
    public async Task<Unit> Handle(UpdateSprintDataCommand command, CancellationToken cancellationToken)
    {
        await sprintCommandsService.UpdateSprintData(command);
        return Unit.Value;
    }
}