namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebDocuIntervalChangedNotification(
    int DocumentationSaveInterval,
    Guid TraceId);