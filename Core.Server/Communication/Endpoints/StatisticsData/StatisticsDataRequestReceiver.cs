using Core.Server.Services.Entities.StatisticsData;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.DTO.StatisticsData;
using Proto.Requests.StatisticsData;

namespace Core.Server.Communication.Endpoints.StatisticsData;

[Authorize]
public class StatisticsDataRequestReceiver(IStatisticsDataRequestsService statisticsDataRequests)
    : StatisticsDataRequestService.StatisticsDataRequestServiceBase
{
    public override async Task<StatisticsDataProto> GetStatisticsDataByTimeSlotId(
        GetStatisticsDataByTimeSlotIdRequestProto request, ServerCallContext context)
    {
        var statisticsData = await statisticsDataRequests.GetStatisticsDataByTimeSlotId(Guid.Parse(request.TimeSlotId));
        return statisticsData.ToProto();
    }
}