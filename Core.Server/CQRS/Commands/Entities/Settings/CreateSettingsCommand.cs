
namespace Application.Contract.CQRS.Commands.Entities.Settings;

public record CreateSettingsCommand(
    Guid SettingsId,
    string ExportPath,
    bool IsAddNewTicketsToCurrentSprintActive);