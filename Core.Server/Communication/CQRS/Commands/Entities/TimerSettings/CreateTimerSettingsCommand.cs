namespace Core.Server.Communication.CQRS.Commands.Entities.TimerSettings;

public record CreateTimerSettingsCommand(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval);