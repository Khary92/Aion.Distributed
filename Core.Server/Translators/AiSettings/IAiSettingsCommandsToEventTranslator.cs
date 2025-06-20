using Domain.Events.AiSettings;
using Service.Server.Communication.CQRS.Commands.Entities.AiSettings;

namespace Service.Server.Translators.AiSettings;

public interface IAiSettingsCommandsToEventTranslator
{
    AiSettingsEvent ToEvent(CreateAiSettingsCommand command);
    AiSettingsEvent ToEvent(ChangePromptCommand command);
    AiSettingsEvent ToEvent(ChangeLanguageModelCommand command);
}