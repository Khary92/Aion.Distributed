namespace Service.Admin.Web.Communication.Tickets.Records;

public record WebCreateTicketCommand(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds,
    Guid TraceId);