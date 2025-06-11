using System;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Factories;

public class StatisticsViewModelFactory(IServiceProvider serviceProvider) : IStatisticsViewModelFactory
{
    public StatisticsViewModel Create()
    {
        var documentationViewModel =
            serviceProvider.GetRequiredService<StatisticsViewModel>();
        return documentationViewModel;
    }
}