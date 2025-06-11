using Client.Avalonia.Models.TimeTracking.DynamicControls;

namespace Client.Avalonia.Factories;

public interface IStatisticsViewModelFactory
{
    StatisticsViewModel Create();
}