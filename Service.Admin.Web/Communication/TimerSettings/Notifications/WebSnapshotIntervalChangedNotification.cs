namespace Service.Admin.Web.Communication.TimerSettings.Notifications;

public record WebSnapshotIntervalChangedNotification(int SnapshotSaveInterval, Guid TraceId);