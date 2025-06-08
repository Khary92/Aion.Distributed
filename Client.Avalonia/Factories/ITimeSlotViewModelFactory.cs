using Client.Avalonia.ViewModels.TimeTracking;

namespace Client.Avalonia.Factories;

public interface ITimeSlotViewModelFactory
{
    TimeSlotViewModel Create();
}