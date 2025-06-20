using Service.Server.CQRS.Requests.StatisticsData;
using Service.Server.Old.Services.Entities.StatisticsData;

namespace Service.Server.Old.Handler.Requests.StatisticsData;

public class GetStatisticsDataByTimeSlotIdRequestHandler(IStatisticsDataRequestsService statisticsDataRequestsService)
    : IRequestHandler<GetStatisticsDataByTimeSlotIdRequest, StatisticsDataDto>
{
    public async Task<StatisticsDataDto> Handle(GetStatisticsDataByTimeSlotIdRequest request,
        CancellationToken cancellationToken)
    {
        return await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(request.TimeSlotId);
    }
}