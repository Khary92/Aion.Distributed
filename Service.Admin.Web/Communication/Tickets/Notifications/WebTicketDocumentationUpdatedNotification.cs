namespace Service.Admin.Web.Communication.Tickets.Notifications;

public record WebTicketDocumentationUpdatedNotification(Guid TicketId, Guid TraceId);