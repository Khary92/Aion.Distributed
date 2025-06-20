namespace Core.Server.Communication.CQRS.Commands.Entities.Tickets;

public record UpdateTicketDataCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds);