namespace Core.Server.Communication.Records.Commands.Entities.Tickets;

public record UpdateTicketDocumentationCommand(Guid TicketId, string Documentation, Guid TraceId);