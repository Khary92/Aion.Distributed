using System;
using Client.Avalonia.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Avalonia.Factories;

public class StatisticsViewModelFactory(IServiceProvider serviceProvider) : IStatisticsViewModelFactory
{
    public StatisticsViewModel Create()
    {
        var documentationViewModel =
            serviceProvider.GetRequiredService<StatisticsViewModel>();
        return documentationViewModel;
    }
}