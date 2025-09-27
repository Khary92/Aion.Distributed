namespace Service.Admin.Web.Communication.Records.Commands;

public record WebCreateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds,
    Guid TraceId);