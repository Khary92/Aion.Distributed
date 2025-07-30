namespace Core.Server.Communication.Records.Commands.Entities.AiSettings;

public record CreateAiSettingsCommand(
    Guid AiSettingsId,
    string Prompt,
    string LanguageModelPath,
    Guid TraceId);