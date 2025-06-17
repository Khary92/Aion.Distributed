using Application.Contract.CQRS.Requests.TimeSlots;
using Application.Contract.DTO;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Requests.TimeSlots;

public class GetTimeSlotsForWorkDayIdRequestHandler(ITimeSlotRequestsService timeSlotRequestsService)
    : IRequestHandler<GetTimeSlotsForWorkDayIdRequest, List<TimeSlotDto>>
{
    public async Task<List<TimeSlotDto>> Handle(GetTimeSlotsForWorkDayIdRequest request,
        CancellationToken cancellationToken)
    {
        return await timeSlotRequestsService.GetTimeSlotsForWorkDayId(request.WorkDayId);
    }
}