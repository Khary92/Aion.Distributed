using Service.Server.CQRS.Requests.TimeSlots;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Requests.TimeSlots;

public class GetTimeSlotByIdRequestHandler(ITimeSlotRequestsService timeSlotRequestsService)
    : IRequestHandler<GetTimeSlotByIdRequest, TimeSlotDto>
{
    public async Task<TimeSlotDto> Handle(GetTimeSlotByIdRequest request, CancellationToken cancellationToken)
    {
        return await timeSlotRequestsService.GetById(request.TimeSlotId);
    }
}