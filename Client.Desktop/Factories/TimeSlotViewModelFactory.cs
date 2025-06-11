using System;
using Client.Desktop.Models.TimeTracking;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Factories;

public class TimeSlotViewModelFactory(IServiceProvider serviceProvider) : ITimeSlotViewModelFactory
{
    public TimeSlotViewModel Create()
    {
        var timeSlotViewModel = serviceProvider.GetRequiredService<TimeSlotViewModel>();
        return timeSlotViewModel;
    }
}