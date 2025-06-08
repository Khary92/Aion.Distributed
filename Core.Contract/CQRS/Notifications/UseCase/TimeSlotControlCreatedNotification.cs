using MediatR;

namespace Contract.CQRS.Notifications.UseCase;

public record TimeSlotControlCreatedNotification(Guid ViewId, Guid TimeSlotId, Guid TicketId)
    : INotification;