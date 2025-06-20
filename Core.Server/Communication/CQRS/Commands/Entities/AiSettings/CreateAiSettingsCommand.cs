namespace Core.Server.Communication.CQRS.Commands.Entities.AiSettings;

public record CreateAiSettingsCommand(
    Guid AiSettingsId,
    string Prompt,
    string LanguageModelPath);