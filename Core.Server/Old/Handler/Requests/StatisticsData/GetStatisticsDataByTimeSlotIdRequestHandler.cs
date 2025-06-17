using Application.Contract.CQRS.Requests.StatisticsData;
using Application.Contract.DTO;
using Application.Services.Entities.StatisticsData;
using MediatR;

namespace Application.Handler.Requests.StatisticsData;

public class GetStatisticsDataByTimeSlotIdRequestHandler(IStatisticsDataRequestsService statisticsDataRequestsService)
    : IRequestHandler<GetStatisticsDataByTimeSlotIdRequest, StatisticsDataDto>
{
    public async Task<StatisticsDataDto> Handle(GetStatisticsDataByTimeSlotIdRequest request,
        CancellationToken cancellationToken)
    {
        return await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(request.TimeSlotId);
    }
}