namespace Core.Server.Communication.CQRS.Commands.Entities.Tickets;

public record CreateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds);