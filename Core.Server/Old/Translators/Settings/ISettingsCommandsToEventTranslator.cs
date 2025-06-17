using Application.Contract.CQRS.Commands.Entities.Settings;
using Domain.Events.Settings;

namespace Application.Translators.Settings;

public interface ISettingsCommandsToEventTranslator
{
    SettingsEvent ToEvent(CreateSettingsCommand command);
    SettingsEvent ToEvent(UpdateSettingsCommand command);
}