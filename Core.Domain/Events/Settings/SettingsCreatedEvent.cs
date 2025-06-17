namespace Domain.Events.Settings;

public record SettingsCreatedEvent(
    Guid SettingsId,
    string ExportPath,
    bool IsAddNewTicketsToCurrentSprintActive);