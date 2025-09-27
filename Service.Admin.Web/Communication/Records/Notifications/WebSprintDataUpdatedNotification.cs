namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebSprintDataUpdatedNotification(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    Guid TraceId);