using System;
using System.Threading.Tasks;
using Client.Desktop.Models.TimeTracking;

namespace Client.Desktop.Factories;

public interface ITimeSlotViewModelFactory
{
    Task<TimeSlotViewModel> Create(Guid ticketId, Guid statisticsDataId, Guid timeSlotId);
}