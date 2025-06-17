using System.Collections.ObjectModel;

namespace Application.Contract.CQRS.Commands.Entities.Tickets;

public record CreateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    Collection<Guid> SprintIds);