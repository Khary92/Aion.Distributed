using Core.Server.Communication.CQRS.Commands.Entities.StatisticsData;

namespace Core.Server.Services.Entities.StatisticsData;

public interface IStatisticsDataCommandsService
{
    Task ChangeTagSelection(ChangeTagSelectionCommand changeTagSelectionCommand);
    Task ChangeProductivity(ChangeProductivityCommand changeProductivityCommand);
    Task Create(CreateStatisticsDataCommand createStatisticsDataCommand);
}