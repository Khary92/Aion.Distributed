namespace Core.Server.Communication.Records.Commands.Entities.Settings;

public record ChangeExportPathCommand(
    Guid SettingsId,
    string ExportPath,
    Guid TraceId);