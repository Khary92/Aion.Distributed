namespace Core.Server.Communication.Records.Commands.Entities.Tickets;

public record CreateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds, 
    Guid TraceId);