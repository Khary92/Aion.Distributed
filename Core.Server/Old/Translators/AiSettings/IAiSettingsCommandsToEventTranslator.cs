using Domain.Events.AiSettings;
using Service.Server.CQRS.Commands.Entities.AiSettings;

namespace Service.Server.Old.Translators.AiSettings;

public interface IAiSettingsCommandsToEventTranslator
{
    AiSettingsEvent ToEvent(CreateAiSettingsCommand command);
    AiSettingsEvent ToEvent(ChangePromptCommand command);
    AiSettingsEvent ToEvent(ChangeLanguageModelCommand command);
}