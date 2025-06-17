using Application.Contract.CQRS.Commands.Entities.StatisticsData;
using Application.Services.Entities.StatisticsData;
using MediatR;

namespace Application.Handler.Commands.Entities.StatisticsData;

public class ChangeTagSelectionCommandHandler(IStatisticsDataCommandsService statisticsDataCommandsService)
    : IRequestHandler<ChangeTagSelectionCommand, Unit>
{
    public async Task<Unit> Handle(ChangeTagSelectionCommand command, CancellationToken cancellationToken)
    {
        await statisticsDataCommandsService.ChangeTagSelection(command);
        return Unit.Value;
    }
}