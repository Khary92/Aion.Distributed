using Application.Contract.CQRS.Commands.Entities.StatisticsData;
using Application.Services.Entities.StatisticsData;
using MediatR;

namespace Application.Handler.Commands.Entities.StatisticsData;

public class CreateStatisticsDataCommandHandler(IStatisticsDataCommandsService statisticsDataCommandsService)
    : IRequestHandler<CreateStatisticsDataCommand, Unit>
{
    public async Task<Unit> Handle(CreateStatisticsDataCommand command, CancellationToken cancellationToken)
    {
        await statisticsDataCommandsService.Create(command);
        return Unit.Value;
    }
}