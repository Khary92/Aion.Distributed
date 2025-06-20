namespace Core.Server.Communication.CQRS.Commands.Entities.Settings;

public record ChangeExportPathCommand(
    Guid SettingsId,
    string ExportPath);