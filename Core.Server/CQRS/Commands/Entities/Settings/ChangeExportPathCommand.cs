namespace Service.Server.CQRS.Commands.Entities.Settings;

public record ChangeExportPathCommand(Guid SettingsId,
    string ExportPath);