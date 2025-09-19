using System;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Factories;

public class StatisticsViewModelFactory(IServiceProvider serviceProvider) : IStatisticsViewModelFactory
{
    public StatisticsViewModel Create(StatisticsDataClientModel statisticsData)
    {
        var statisticsViewModel =
            serviceProvider.GetRequiredService<StatisticsViewModel>();

        statisticsViewModel.StatisticsData = statisticsData;
        return statisticsViewModel;
    }
}