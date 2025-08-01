namespace Service.Admin.Web.Communication.TimerSettings.Notifications;

public record WebSnapshotIntervalChangedNotification(
    Guid TimerSettingsId,
    int SnapshotSaveInterval,
    Guid TraceId);