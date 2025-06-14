using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public interface IStatisticsDataRequestSender
{
    Task<StatisticsDataDto> Send(GetStatisticsDataByTimeSlotIdRequestProto request);
}