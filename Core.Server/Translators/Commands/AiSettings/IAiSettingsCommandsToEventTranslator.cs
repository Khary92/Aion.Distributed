using Core.Server.Communication.Records.Commands.Entities.AiSettings;
using Domain.Events.AiSettings;

namespace Core.Server.Translators.Commands.AiSettings;

public interface IAiSettingsCommandsToEventTranslator
{
    AiSettingsEvent ToEvent(CreateAiSettingsCommand command);
    AiSettingsEvent ToEvent(ChangePromptCommand command);
    AiSettingsEvent ToEvent(ChangeLanguageModelCommand command);
}