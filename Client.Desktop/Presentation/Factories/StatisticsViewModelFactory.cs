using System;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Factories;

public class StatisticsViewModelFactory(IServiceProvider serviceProvider) : IStatisticsViewModelFactory
{
    public StatisticsViewModel Create()
    {
        var documentationViewModel =
            serviceProvider.GetRequiredService<StatisticsViewModel>();
        return documentationViewModel;
    }
}