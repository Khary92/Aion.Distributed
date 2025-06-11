using System.Threading.Tasks;
using Proto.Requests.StatisticsData;

namespace Client.Avalonia.Communication.Requests.StatisticsData;

public interface IStatisticsDataRequestSender
{
    Task<StatisticsDataProto> GetByTimeSlotId(string timeSlotId);
}