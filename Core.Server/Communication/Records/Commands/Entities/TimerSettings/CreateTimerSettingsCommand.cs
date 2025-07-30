namespace Core.Server.Communication.Records.Commands.Entities.TimerSettings;

public record CreateTimerSettingsCommand(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval,
    Guid TraceId);