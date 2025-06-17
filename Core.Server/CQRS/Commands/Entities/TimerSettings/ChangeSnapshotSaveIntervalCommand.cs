
namespace Application.Contract.CQRS.Commands.Entities.TimerSettings;

public record ChangeSnapshotSaveIntervalCommand(
    Guid TimerSettingsId,
    int SnapshotSaveInterval);