using System.Collections.ObjectModel;
using MediatR;

namespace Contract.CQRS.Notifications.Entities.Tickets;

public record TicketCreatedNotification(
    Guid TicketId,
    string Name,
    string BookingNumber,
    Collection<Guid> SprintIds)
    : IRequest<Unit>, INotification;