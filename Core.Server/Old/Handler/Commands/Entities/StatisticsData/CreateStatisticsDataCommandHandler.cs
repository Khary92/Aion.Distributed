using Service.Server.CQRS.Commands.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.StatisticsData;

namespace Service.Server.Old.Handler.Commands.Entities.StatisticsData;

public class CreateStatisticsDataCommandHandler(IStatisticsDataCommandsService statisticsDataCommandsService)
    : IRequestHandler<CreateStatisticsDataCommand, Unit>
{
    public async Task<Unit> Handle(CreateStatisticsDataCommand command, CancellationToken cancellationToken)
    {
        await statisticsDataCommandsService.Create(command);
        return Unit.Value;
    }
}