using Grpc.Core;
using Proto.DTO.StatisticsData;
using Proto.Requests.StatisticsData;
using Service.Server.Services.Entities.StatisticsData;

namespace Service.Server.Communication.Services.StatisticsData;

public class StatisticsDataRequestReceiver(IStatisticsDataRequestsService statisticsDataRequests)
    : StatisticsDataRequestService.StatisticsDataRequestServiceBase
{
    public override async Task<StatisticsDataProto> GetStatisticsDataByTimeSlotId(
        GetStatisticsDataByTimeSlotIdRequestProto request, ServerCallContext context)
    {
        var statisticsData = await statisticsDataRequests.GetStatisticsDataByTimeSlotId(Guid.Parse( request.TimeSlotId));
        return statisticsData.ToProto();
    }
}