using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public interface IStatisticsDataRequestSender
{
    Task<StatisticsDataDto> Send(GetStatisticsDataByTimeSlotIdRequestProto request);
}