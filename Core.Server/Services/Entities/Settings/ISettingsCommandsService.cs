using Core.Server.Communication.CQRS.Commands.Entities.Settings;

namespace Core.Server.Services.Entities.Settings;

public interface ISettingsCommandsService
{
    Task ChangeAutomaticTicketAddingToSprint(ChangeAutomaticTicketAddingToSprintCommand command);
    Task ChangeExportPath(ChangeExportPathCommand command);
    Task Create(CreateSettingsCommand createSettingsCommand);
}