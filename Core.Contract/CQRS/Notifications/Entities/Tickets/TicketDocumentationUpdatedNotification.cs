using MediatR;

namespace Contract.CQRS.Notifications.Entities.Tickets;

public record TicketDocumentationUpdatedNotification(Guid TicketId, string Documentation)
    : IRequest<Unit>, INotification;