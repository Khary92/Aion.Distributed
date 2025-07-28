using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking;

namespace Client.Desktop.Presentation.Factories;

public interface ITimeSlotViewModelFactory
{
    Task<TimeSlotViewModel> Create(TicketClientModel ticket, StatisticsDataClientModel statisticsData,
        TimeSlotClientModel timeSlot);
}