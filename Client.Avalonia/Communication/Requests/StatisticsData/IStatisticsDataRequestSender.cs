using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.StatisticsData;

namespace Client.Avalonia.Communication.Requests.StatisticsData;

public interface IStatisticsDataRequestSender
{
    Task<StatisticsDataDto> GetByTimeSlotId(string timeSlotId);
}