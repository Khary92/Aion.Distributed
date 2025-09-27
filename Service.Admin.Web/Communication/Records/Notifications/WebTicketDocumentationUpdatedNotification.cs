namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebTicketDocumentationUpdatedNotification(Guid TicketId, Guid TraceId);