using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.StatisticsData.Records;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public interface IStatisticsDataRequestSender
{
    Task<StatisticsDataClientModel> Send(ClientGetStatisticsDataByTimeSlotIdRequest request);
}