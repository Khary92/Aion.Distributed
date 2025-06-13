using Grpc.Core;
using Proto.Requests.StatisticsData;

public class StatisticsDataRequestServiceImpl : StatisticsDataRequestService.StatisticsDataRequestServiceBase
{
    public override Task<StatisticsDataProto> GetStatisticsDataByTimeSlotId(
        GetStatisticsDataByTimeSlotIdRequestProto request, ServerCallContext context)
    {
        var response = new StatisticsDataProto
        {
            StatisticsId = "stats-001",
            TimeSlotId = request.TimeSlotId,
            IsProductive = true,
            IsNeutral = false,
            IsUnproductive = false
        };
        response.TagIds.Add("tag-1");
        response.TagIds.Add("tag-2");

        return Task.FromResult(response);
    }
}