using Service.Server.CQRS.Commands.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.StatisticsData;

namespace Service.Server.Old.Handler.Commands.Entities.StatisticsData;

public class ChangeTagSelectionCommandHandler(IStatisticsDataCommandsService statisticsDataCommandsService)
    : IRequestHandler<ChangeTagSelectionCommand, Unit>
{
    public async Task<Unit> Handle(ChangeTagSelectionCommand command, CancellationToken cancellationToken)
    {
        await statisticsDataCommandsService.ChangeTagSelection(command);
        return Unit.Value;
    }
}