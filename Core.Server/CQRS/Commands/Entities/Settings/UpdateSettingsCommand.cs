
namespace Service.Server.CQRS.Commands.Entities.Settings;

public record UpdateSettingsCommand(
    Guid SettingsId,
    string ExportPath,
    bool IsAddNewTicketsToCurrentSprintActive);