using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Presentation.Factories;

public interface IStatisticsViewModelFactory
{
    StatisticsViewModel Create(StatisticsDataClientModel statisticsData);
}