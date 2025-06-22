namespace Core.Server.Communication.Records.Commands.Entities.Settings;

public record CreateSettingsCommand(
    Guid SettingsId,
    string ExportPath,
    bool IsAddNewTicketsToCurrentSprintActive);