using MediatR;

namespace Contract.CQRS.Commands.Entities.TimeSlots;

public record SetEndTimeCommand(Guid TimeSlotId, DateTimeOffset Time) : INotification, IRequest<Unit>;