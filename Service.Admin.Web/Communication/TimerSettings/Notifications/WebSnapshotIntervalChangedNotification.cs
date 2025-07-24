namespace Service.Admin.Web.Communication.TimerSettings.Notifications.TimerSettings;

public record WebSnapshotIntervalChangedNotification(
    Guid TimerSettingsId,
    int SnapshotSaveInterval);