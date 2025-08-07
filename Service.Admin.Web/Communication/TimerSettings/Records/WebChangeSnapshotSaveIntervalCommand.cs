namespace Service.Admin.Web.Communication.TimerSettings.Records;

public record WebChangeSnapshotSaveIntervalCommand(Guid TimerSettingsId, int SnapshotSaveInterval, Guid TraceId);