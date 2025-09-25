using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking;

namespace Client.Desktop.Presentation.Factories;

public interface ITrackingSlotViewModelFactory
{
    Task<TrackingSlotViewModel> Create(TicketClientModel ticket, StatisticsDataClientModel statisticsData,
        TimeSlotClientModel timeSlot);
}