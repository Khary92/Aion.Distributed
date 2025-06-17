using Application.Contract.CQRS.Commands.Entities.StatisticsData;

namespace Application.Services.Entities.StatisticsData;

public interface IStatisticsDataCommandsService
{
    Task ChangeTagSelection(ChangeTagSelectionCommand changeTagSelectionCommand);
    Task ChangeProductivity(ChangeProductivityCommand changeProductivityCommand);
    Task Create(CreateStatisticsDataCommand createStatisticsDataCommand);
}