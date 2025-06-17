namespace Domain.Events.AiSettings;

public record LanguageModelChangedEvent(Guid AiSettingsId, string LanguageModelPath);