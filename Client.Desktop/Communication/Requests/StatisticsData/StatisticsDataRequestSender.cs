using System;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.StatisticsData.Records;
using Client.Desktop.DataModels;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public class StatisticsDataRequestSender : IStatisticsDataRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly StatisticsDataRequestService.StatisticsDataRequestServiceClient _client = new(Channel);

    public async Task<StatisticsDataClientModel> Send(ClientGetStatisticsDataByTimeSlotIdRequest request)
    {
        var response = await _client.GetStatisticsDataByTimeSlotIdAsync(request.ToProto());
        return response.ToClientModel();
    }
}