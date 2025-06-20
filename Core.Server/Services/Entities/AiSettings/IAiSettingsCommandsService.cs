using Service.Server.Communication.CQRS.Commands.Entities.AiSettings;

namespace Service.Server.Services.Entities.AiSettings;

public interface IAiSettingsCommandsService
{
    Task Create(CreateAiSettingsCommand @event);
    Task ChangePrompt(ChangePromptCommand @event);
    Task ChangeLanguageModelPath(ChangeLanguageModelCommand @event);
}