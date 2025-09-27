namespace Service.Admin.Web.Communication.Records.Commands;

public record WebCreateTimerSettingsCommand(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval,
    Guid TraceId);