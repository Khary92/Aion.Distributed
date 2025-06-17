namespace Domain.Events.TimerSettings;

public record DocuIntervalChangedEvent(
    Guid TimerSettingsId,
    int DocumentationSaveInterval);