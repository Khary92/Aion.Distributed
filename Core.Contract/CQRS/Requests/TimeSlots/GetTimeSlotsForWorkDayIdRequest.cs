using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.TimeSlots;

public record GetTimeSlotsForWorkDayIdRequest(Guid WorkDayId) : IRequest<List<TimeSlotDto>>, INotification;