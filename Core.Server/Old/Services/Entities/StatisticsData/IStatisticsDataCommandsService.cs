using Service.Server.CQRS.Commands.Entities.StatisticsData;

namespace Service.Server.Old.Services.Entities.StatisticsData;

public interface IStatisticsDataCommandsService
{
    Task ChangeTagSelection(ChangeTagSelectionCommand changeTagSelectionCommand);
    Task ChangeProductivity(ChangeProductivityCommand changeProductivityCommand);
    Task Create(CreateStatisticsDataCommand createStatisticsDataCommand);
}