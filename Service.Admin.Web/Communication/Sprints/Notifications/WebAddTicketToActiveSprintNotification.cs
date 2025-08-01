namespace Service.Admin.Web.Communication.Sprints.Notifications;

public record WebAddTicketToActiveSprintNotification(Guid TicketId, Guid TraceId);