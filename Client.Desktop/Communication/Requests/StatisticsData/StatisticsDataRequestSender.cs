using System;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Grpc.Net.Client;
using Proto.Client;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public class StatisticsDataRequestSender : IStatisticsDataRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly StatisticsDataRequestService.StatisticsDataRequestServiceClient _client = new(Channel);

    public async Task<StatisticsDataDto> Send(GetStatisticsDataByTimeSlotIdRequestProto request)
    {
        var response = await _client.GetStatisticsDataByTimeSlotIdAsync(request);

        var tagIds = response.TagIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new StatisticsDataDto(Guid.Parse(response.StatisticsId), Guid.Parse(response.TimeSlotId), tagIds,
            response.IsProductive, response.IsNeutral, response.IsUnproductive);
    }
}