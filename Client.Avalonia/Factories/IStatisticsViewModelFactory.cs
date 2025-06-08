using Client.Avalonia.ViewModels.TimeTracking.DynamicControls;

namespace Client.Avalonia.Factories;

public interface IStatisticsViewModelFactory
{
    StatisticsViewModel Create();
}