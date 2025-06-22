namespace Core.Server.Communication.Records.Commands.Entities.TimerSettings;

public record ChangeDocuTimerSaveIntervalCommand(
    Guid TimerSettingsId,
    int DocuTimerSaveInterval);