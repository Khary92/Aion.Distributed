namespace Service.Admin.Web.Communication.Records.Commands;

public record WebUpdateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds,
    Guid TraceId);