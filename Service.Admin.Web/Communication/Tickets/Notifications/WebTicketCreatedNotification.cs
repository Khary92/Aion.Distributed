namespace Service.Admin.Web.Communication.Tickets.Notifications;

public record WebTicketCreatedNotification(
    Guid TicketId,
    string Name,
    string BookingNumber,
    string Documentation,
    List<Guid> SprintIds,
    Guid TraceId);