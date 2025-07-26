using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public interface IStatisticsDataRequestSender
{
    Task<StatisticsDataClientModel> Send(GetStatisticsDataByTimeSlotIdRequestProto request);
}