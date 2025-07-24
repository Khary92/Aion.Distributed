namespace Service.Admin.Web.Communication.Sprints.Notifications;

public record WebAddTicketToSprintNotification(Guid SprintId, Guid TicketId);