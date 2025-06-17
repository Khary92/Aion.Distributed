namespace Domain.Events.AiSettings;

public record AiSettingsCreatedEvent(
    Guid AiSettingsId,
    string Prompt,
    string LanguageModelPath);