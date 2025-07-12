namespace Service.Admin.Web.Communication.Tickets.Commands;

public record WebTicketDocumentationUpdatedNotification(Guid TicketId, string Documentation);