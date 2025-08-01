using Core.Server.Communication.Records.Commands.Entities.StatisticsData;

namespace Core.Server.Services.Entities.StatisticsData;

public interface IStatisticsDataCommandsService
{
    Task ChangeTagSelection(ChangeTagSelectionCommand command);
    Task ChangeProductivity(ChangeProductivityCommand command);
    Task Create(CreateStatisticsDataCommand command);
}