using Application.Contract.CQRS.Requests.TimeSlots;
using Application.Contract.DTO;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Requests.TimeSlots;

public class GetTimeSlotByIdRequestHandler(ITimeSlotRequestsService timeSlotRequestsService)
    : IRequestHandler<GetTimeSlotByIdRequest, TimeSlotDto>
{
    public async Task<TimeSlotDto> Handle(GetTimeSlotByIdRequest request, CancellationToken cancellationToken)
    {
        return await timeSlotRequestsService.GetById(request.TimeSlotId);
    }
}