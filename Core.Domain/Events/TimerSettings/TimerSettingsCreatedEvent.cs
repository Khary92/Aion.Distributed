namespace Domain.Events.TimerSettings;

public record TimerSettingsCreatedEvent(
    Guid TimerSettingsId,
    int DocumentationSaveInterval,
    int SnapshotSaveInterval);