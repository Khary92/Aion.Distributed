using Grpc.Core;
using Proto.Requests.StatisticsData;

public class StatisticsDataRequestServiceImpl : StatisticsDataRequestService.StatisticsDataRequestServiceBase
{
    public override Task<StatisticsDataProto> GetStatisticsDataByTimeSlotId(
        GetStatisticsDataByTimeSlotIdRequestProto request, ServerCallContext context)
    {
        var response = new StatisticsDataProto
        {
            StatisticsId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            IsProductive = true,
            IsNeutral = false,
            IsUnproductive = false
        };
        response.TagIds.Add(Guid.NewGuid().ToString());
        response.TagIds.Add(Guid.NewGuid().ToString());

        return Task.FromResult(response);
    }
}