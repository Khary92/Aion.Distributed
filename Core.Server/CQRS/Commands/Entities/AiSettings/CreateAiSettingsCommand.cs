
namespace Service.Server.CQRS.Commands.Entities.AiSettings;

public record CreateAiSettingsCommand(
    Guid AiSettingsId,
    string Prompt,
    string LanguageModelPath);