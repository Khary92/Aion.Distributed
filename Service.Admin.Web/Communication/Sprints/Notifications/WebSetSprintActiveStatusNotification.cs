namespace Service.Admin.Web.Communication.Sprints.Notifications;

public record WebSetSprintActiveStatusNotification(Guid SprintId, bool IsActive, Guid TraceId);