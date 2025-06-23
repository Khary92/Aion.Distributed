namespace Domain.Events.Settings;

public record ExportPathChangedEvent(Guid SettingsId, string ExportPath);