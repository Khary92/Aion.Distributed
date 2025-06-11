using System;
using Client.Avalonia.Models.TimeTracking;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Avalonia.Factories;

public class TimeSlotViewModelFactory(IServiceProvider serviceProvider) : ITimeSlotViewModelFactory
{
    public TimeSlotViewModel Create()
    {
        var timeSlotViewModel = serviceProvider.GetRequiredService<TimeSlotViewModel>();
        return timeSlotViewModel;
    }
}