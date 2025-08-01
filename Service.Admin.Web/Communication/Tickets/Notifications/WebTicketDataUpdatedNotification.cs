namespace Service.Admin.Web.Communication.Tickets.Notifications;

public record WebTicketDataUpdatedNotification(Guid TicketId, string Name, string BookingNumber, List<Guid> SprintIds, Guid TraceId);