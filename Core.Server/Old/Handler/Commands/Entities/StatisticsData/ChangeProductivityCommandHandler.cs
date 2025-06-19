using Service.Server.CQRS.Commands.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.StatisticsData;

namespace Service.Server.Old.Handler.Commands.Entities.StatisticsData;

public class ChangeProductivityCommandHandler(IStatisticsDataCommandsService statisticsDataCommandsService)
    : IRequestHandler<ChangeProductivityCommand, Unit>
{
    public async Task<Unit> Handle(ChangeProductivityCommand command, CancellationToken cancellationToken)
    {
        await statisticsDataCommandsService.ChangeProductivity(command);
        return Unit.Value;
    }
}