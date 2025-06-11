using Client.Desktop.Models.TimeTracking;

namespace Client.Desktop.Factories;

public interface ITimeSlotViewModelFactory
{
    TimeSlotViewModel Create();
}