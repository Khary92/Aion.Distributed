using Client.Avalonia.Models.TimeTracking;

namespace Client.Avalonia.Factories;

public interface ITimeSlotViewModelFactory
{
    TimeSlotViewModel Create();
}