namespace Service.Admin.Web.Communication.TimerSettings.Records;

public record WebCreateTimerSettingsCommand(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval,
    Guid TraceId);