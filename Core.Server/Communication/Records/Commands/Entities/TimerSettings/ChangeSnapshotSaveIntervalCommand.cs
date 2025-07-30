namespace Core.Server.Communication.Records.Commands.Entities.TimerSettings;

public record ChangeSnapshotSaveIntervalCommand(
    Guid TimerSettingsId,
    int SnapshotSaveInterval,
    Guid TraceId);