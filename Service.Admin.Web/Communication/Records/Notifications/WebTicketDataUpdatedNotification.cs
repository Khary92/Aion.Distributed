namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebTicketDataUpdatedNotification(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds,
    Guid TraceId);