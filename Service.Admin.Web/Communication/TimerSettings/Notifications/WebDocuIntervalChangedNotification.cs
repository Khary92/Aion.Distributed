namespace Service.Admin.Web.Communication.TimerSettings.Notifications;

public record WebDocuIntervalChangedNotification(
    Guid TimerSettingsId,
    int DocumentationSaveInterval, Guid TraceId);