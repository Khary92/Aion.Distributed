using Core.Server.Communication.Records.Commands.Entities.StatisticsData;

namespace Core.Server.Services.Entities.StatisticsData;

public interface IStatisticsDataCommandsService
{
    Task ChangeTagSelection(ChangeTagSelectionCommand changeTagSelectionCommand);
    Task ChangeProductivity(ChangeProductivityCommand changeProductivityCommand);
    Task Create(CreateStatisticsDataCommand createStatisticsDataCommand);
}