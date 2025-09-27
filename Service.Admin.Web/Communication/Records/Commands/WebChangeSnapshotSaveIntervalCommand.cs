namespace Service.Admin.Web.Communication.Records.Commands;

public record WebChangeSnapshotSaveIntervalCommand(Guid TimerSettingsId, int SnapshotSaveInterval, Guid TraceId);