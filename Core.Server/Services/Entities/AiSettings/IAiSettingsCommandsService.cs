using Service.Server.CQRS.Commands.Entities.AiSettings;

namespace Service.Server.Old.Services.Entities.AiSettings;

public interface IAiSettingsCommandsService
{
    Task Create(CreateAiSettingsCommand @event);
    Task ChangePrompt(ChangePromptCommand @event);
    Task ChangeLanguageModelPath(ChangeLanguageModelCommand @event);
}