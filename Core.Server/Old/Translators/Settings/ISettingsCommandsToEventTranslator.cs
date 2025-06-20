using Domain.Events.Settings;
using Service.Server.CQRS.Commands.Entities.Settings;

namespace Service.Server.Old.Translators.Settings;

public interface ISettingsCommandsToEventTranslator
{
    SettingsEvent ToEvent(CreateSettingsCommand command);
    SettingsEvent ToEvent(ChangeExportPathCommand command);
    SettingsEvent ToEvent(ChangeAutomaticTicketAddingToSprintCommand command);
}