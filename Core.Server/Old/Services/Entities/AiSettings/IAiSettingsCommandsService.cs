using Application.Contract.CQRS.Commands.Entities.AiSettings;

namespace Application.Services.Entities.AiSettings;

public interface IAiSettingsCommandsService
{
    Task Create(CreateAiSettingsCommand @event);
    Task ChangePrompt(ChangePromptCommand @event);
    Task ChangeLanguageModelPath(ChangeLanguageModelCommand @event);
}