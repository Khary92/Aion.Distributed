using System.Collections.ObjectModel;

namespace Service.Server.CQRS.Commands.Entities.Tickets;

public record UpdateTicketDataCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    Collection<Guid> SprintIds);