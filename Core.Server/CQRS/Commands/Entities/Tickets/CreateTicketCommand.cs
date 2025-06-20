using System.Collections.ObjectModel;

namespace Service.Server.CQRS.Commands.Entities.Tickets;

public record CreateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds);