namespace Domain.Events.Settings;

public record SettingsUpdatedEvent(
    Guid SettingsId,
    string ExportPath,
    bool IsAddNewTicketsToCurrentSprintActive);