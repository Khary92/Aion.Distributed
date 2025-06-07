using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.TimeSlots;

public record GetTimeSlotByIdRequest(Guid TimeSlotId) : IRequest<TimeSlotDto>, INotification;