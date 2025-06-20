using Core.Server.Communication.CQRS.Commands.Entities.AiSettings;
using Domain.Events.AiSettings;

namespace Core.Server.Translators.AiSettings;

public interface IAiSettingsCommandsToEventTranslator
{
    AiSettingsEvent ToEvent(CreateAiSettingsCommand command);
    AiSettingsEvent ToEvent(ChangePromptCommand command);
    AiSettingsEvent ToEvent(ChangeLanguageModelCommand command);
}