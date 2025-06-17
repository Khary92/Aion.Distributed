namespace Domain.Events.TimerSettings;

public record SnapshotIntervalChangedEvent(
    Guid TimerSettingsId,
    int SnapshotSaveInterval);