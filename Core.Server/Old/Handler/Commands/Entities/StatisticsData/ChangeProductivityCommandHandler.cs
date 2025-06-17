using Application.Contract.CQRS.Commands.Entities.StatisticsData;
using Application.Services.Entities.StatisticsData;
using MediatR;

namespace Application.Handler.Commands.Entities.StatisticsData;

public class ChangeProductivityCommandHandler(IStatisticsDataCommandsService statisticsDataCommandsService)
    : IRequestHandler<ChangeProductivityCommand, Unit>
{
    public async Task<Unit> Handle(ChangeProductivityCommand command, CancellationToken cancellationToken)
    {
        await statisticsDataCommandsService.ChangeProductivity(command);
        return Unit.Value;
    }
}