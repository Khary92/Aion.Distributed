using Application.Contract.CQRS.Commands.Entities.AiSettings;
using Domain.Events.AiSettings;

namespace Application.Translators.AiSettings;

public interface IAiSettingsCommandsToEventTranslator
{
    AiSettingsEvent ToEvent(CreateAiSettingsCommand command);
    AiSettingsEvent ToEvent(ChangePromptCommand command);
    AiSettingsEvent ToEvent(ChangeLanguageModelCommand command);
}