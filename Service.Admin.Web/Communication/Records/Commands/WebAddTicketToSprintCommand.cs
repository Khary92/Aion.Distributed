namespace Service.Admin.Web.Communication.Records.Commands;

public record WebAddTicketToSprintCommand(Guid TicketId, Guid TraceId);