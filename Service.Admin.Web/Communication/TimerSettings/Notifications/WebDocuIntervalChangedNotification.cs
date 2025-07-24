namespace Service.Admin.Web.Communication.TimerSettings.Notifications.TimerSettings;

public record WebDocuIntervalChangedNotification(
    Guid TimerSettingsId,
    int DocumentationSaveInterval);