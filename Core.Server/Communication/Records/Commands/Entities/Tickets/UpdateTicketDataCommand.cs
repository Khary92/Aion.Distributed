namespace Core.Server.Communication.Records.Commands.Entities.Tickets;

public record UpdateTicketDataCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds, 
    Guid TraceId);