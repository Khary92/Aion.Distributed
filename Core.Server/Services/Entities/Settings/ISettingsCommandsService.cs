using Service.Server.CQRS.Commands.Entities.Settings;

namespace Service.Server.Old.Services.Entities.Settings;

public interface ISettingsCommandsService
{
    Task ChangeAutomaticTicketAddingToSprint(ChangeAutomaticTicketAddingToSprintCommand command);
    Task ChangeExportPath(ChangeExportPathCommand command);
    Task Create(CreateSettingsCommand createSettingsCommand);
}