
namespace Service.Server.CQRS.Commands.Entities.TimerSettings;

public record ChangeSnapshotSaveIntervalCommand(
    Guid TimerSettingsId,
    int SnapshotSaveInterval);