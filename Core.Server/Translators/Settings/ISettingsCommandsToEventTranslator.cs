using Core.Server.Communication.CQRS.Commands.Entities.Settings;
using Domain.Events.Settings;

namespace Core.Server.Translators.Settings;

public interface ISettingsCommandsToEventTranslator
{
    SettingsEvent ToEvent(CreateSettingsCommand command);
    SettingsEvent ToEvent(ChangeExportPathCommand command);
    SettingsEvent ToEvent(ChangeAutomaticTicketAddingToSprintCommand command);
}