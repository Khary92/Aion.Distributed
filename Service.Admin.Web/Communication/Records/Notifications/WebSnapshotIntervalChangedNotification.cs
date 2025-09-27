namespace Service.Admin.Web.Communication.Records.Notifications;

public record WebSnapshotIntervalChangedNotification(int SnapshotSaveInterval, Guid TraceId);