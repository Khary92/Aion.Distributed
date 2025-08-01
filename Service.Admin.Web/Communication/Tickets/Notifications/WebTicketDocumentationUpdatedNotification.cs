namespace Service.Admin.Web.Communication.Tickets.Notifications;

public record WebTicketDocumentationUpdatedNotification(Guid TicketId, string Documentation, Guid TraceId);