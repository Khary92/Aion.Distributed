using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.StatisticsData;

namespace Client.Avalonia.Communication.Requests.StatisticsData;

public class StatisticsDataRequestSender : IStatisticsDataRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly StatisticsDataRequestService.StatisticsDataRequestServiceClient _client = new(Channel);

    public async Task<StatisticsDataProto> GetByTimeSlotId(string timeSlotId)
    {
        var request = new GetStatisticsDataByTimeSlotIdRequestProto { TimeSlotId = timeSlotId };
        var response = await _client.GetStatisticsDataByTimeSlotIdAsync(request);
        return response;
    }
}