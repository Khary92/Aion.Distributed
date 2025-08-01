namespace Service.Admin.Web.Communication.Sprints.Notifications;

public record WebSprintDataUpdatedNotification(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    Guid TraceId);