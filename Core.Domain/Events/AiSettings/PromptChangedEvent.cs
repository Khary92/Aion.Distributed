namespace Domain.Events.AiSettings;

public record PromptChangedEvent(Guid AiSettingsId, string Prompt);