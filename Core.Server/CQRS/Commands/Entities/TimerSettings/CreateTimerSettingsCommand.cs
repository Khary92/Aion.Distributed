
namespace Service.Server.CQRS.Commands.Entities.TimerSettings;

public record CreateTimerSettingsCommand(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval);