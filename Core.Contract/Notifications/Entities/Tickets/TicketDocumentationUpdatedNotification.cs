using MediatR;

namespace Contract.Notifications.Entities.Tickets;

public record TicketDocumentationUpdatedNotification(Guid TicketId, string Documentation)
    : IRequest<Unit>, INotification;