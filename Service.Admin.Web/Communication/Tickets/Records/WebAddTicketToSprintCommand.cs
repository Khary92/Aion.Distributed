namespace Service.Admin.Web.Communication.Tickets.Records;

public record WebAddTicketToSprintCommand(Guid TicketId, Guid TraceId);