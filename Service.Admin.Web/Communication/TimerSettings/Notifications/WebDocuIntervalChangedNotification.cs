namespace Service.Admin.Web.Communication.TimerSettings.Notifications;

public record WebDocuIntervalChangedNotification(
    int DocumentationSaveInterval,
    Guid TraceId);