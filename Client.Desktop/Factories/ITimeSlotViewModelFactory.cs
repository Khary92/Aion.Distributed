using System;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.Models.TimeTracking;

namespace Client.Desktop.Factories;

public interface ITimeSlotViewModelFactory
{
    Task<TimeSlotViewModel> Create(TicketDto ticket, StatisticsDataDto statisticsData, TimeSlotDto timeSlot);
}