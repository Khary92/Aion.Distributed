using MediatR;

namespace Contract.CQRS.Commands.Entities.TimeSlots;

public record SetStartTimeCommand(Guid TimeSlotId, DateTimeOffset Time) : INotification, IRequest<Unit>;