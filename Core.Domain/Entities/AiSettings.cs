using System.Text.Json;
using Domain.Events.AiSettings;

namespace Domain.Entities;

public class AiSettings
{
    public Guid AiSettingsId { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string LanguageModelPath { get; set; } = string.Empty;

    public static AiSettings Rehydrate(IEnumerable<AiSettingsEvent> events)
    {
        var aiSettings = new AiSettings();

        foreach (var evt in events) aiSettings.Apply(evt);

        return aiSettings;
    }

    private void Apply(AiSettingsEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(AiSettingsCreatedEvent):
                var created = JsonSerializer.Deserialize<AiSettingsCreatedEvent>(evt.EventPayload);
                AiSettingsId = created!.AiSettingsId;
                Prompt = created.Prompt;
                LanguageModelPath = created.LanguageModelPath;
                break;

            case nameof(PromptChangedEvent):
                var prompt = JsonSerializer.Deserialize<PromptChangedEvent>(evt.EventPayload);
                Prompt = prompt!.Prompt;
                break;

            case nameof(LanguageModelChangedEvent):
                var languageModelChanged = JsonSerializer.Deserialize<LanguageModelChangedEvent>(evt.EventPayload);
                LanguageModelPath = languageModelChanged!.LanguageModelPath;
                break;
        }
    }
}