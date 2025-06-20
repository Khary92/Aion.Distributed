using Service.Server.CQRS.Requests.TimeSlots;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Requests.TimeSlots;

public class GetTimeSlotsForWorkDayIdRequestHandler(ITimeSlotRequestsService timeSlotRequestsService)
    : IRequestHandler<GetTimeSlotsForWorkDayIdRequest, List<TimeSlotDto>>
{
    public async Task<List<TimeSlotDto>> Handle(GetTimeSlotsForWorkDayIdRequest request,
        CancellationToken cancellationToken)
    {
        return await timeSlotRequestsService.GetTimeSlotsForWorkDayId(request.WorkDayId);
    }
}