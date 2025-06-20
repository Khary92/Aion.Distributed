
namespace Service.Server.Communication.CQRS.Commands.Entities.TimerSettings;

public record ChangeSnapshotSaveIntervalCommand(
    Guid TimerSettingsId,
    int SnapshotSaveInterval);