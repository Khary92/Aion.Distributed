
namespace Application.Contract.CQRS.Commands.Entities.AiSettings;

public record CreateAiSettingsCommand(
    Guid AiSettingsId,
    string Prompt,
    string LanguageModelPath);