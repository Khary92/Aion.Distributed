using Core.Server.Communication.Records.Commands.Entities.Settings;
using Domain.Events.Settings;

namespace Core.Server.Translators.Commands.Settings;

public interface ISettingsCommandsToEventTranslator
{
    SettingsEvent ToEvent(CreateSettingsCommand command);
    SettingsEvent ToEvent(ChangeExportPathCommand command);
    SettingsEvent ToEvent(ChangeAutomaticTicketAddingToSprintCommand command);
}