using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.StatisticsData.Records;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockStatisticsDataRequestSender(MockDataService mockDataService) : IStatisticsDataRequestSender
{
    public Task<StatisticsDataClientModel> Send(ClientGetStatisticsDataByTimeSlotIdRequest request)
    {
        return Task.FromResult(mockDataService.StatisticsData.First(sd => sd.TimeSlotId == request.TimeSlotId));
    }
}