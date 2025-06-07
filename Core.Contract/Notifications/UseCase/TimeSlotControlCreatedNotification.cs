using MediatR;

namespace Contract.Notifications.UseCase;

public record TimeSlotControlCreatedNotification(Guid ViewId, Guid TimeSlotId, Guid TicketId)
    : INotification;