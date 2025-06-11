using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public interface IStatisticsDataRequestSender
{
    Task<StatisticsDataDto> GetByTimeSlotId(string timeSlotId);
}