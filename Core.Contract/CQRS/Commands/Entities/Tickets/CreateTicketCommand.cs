using System.Collections.ObjectModel;
using MediatR;

namespace Contract.CQRS.Commands.Entities.Tickets;

public record CreateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    Collection<Guid> SprintIds)
    : IRequest<Unit>, INotification;