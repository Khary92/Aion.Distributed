using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.StatisticsData.Records;
using Client.Desktop.DataModels;
using Grpc.Net.Client;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public class StatisticsDataRequestSender : IStatisticsDataRequestSender
{
    private readonly StatisticsDataRequestService.StatisticsDataRequestServiceClient _client;

    public StatisticsDataRequestSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new StatisticsDataRequestService.StatisticsDataRequestServiceClient(channel);
    }

    public async Task<StatisticsDataClientModel> Send(ClientGetStatisticsDataByTimeSlotIdRequest request)
    {
        var response = await _client.GetStatisticsDataByTimeSlotIdAsync(request.ToProto());
        return response.ToClientModel();
    }
}