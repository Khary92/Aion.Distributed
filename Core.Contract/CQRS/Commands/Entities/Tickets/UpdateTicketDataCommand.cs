using System.Collections.ObjectModel;
using MediatR;

namespace Contract.CQRS.Commands.Entities.Tickets;

public record UpdateTicketDataCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    Collection<Guid> SprintIds)
    : IRequest<Unit>, INotification;