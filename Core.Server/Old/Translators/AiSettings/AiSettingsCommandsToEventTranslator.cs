using System.Text.Json;
using Application.Contract.CQRS.Commands.Entities.AiSettings;
using Domain.Events.AiSettings;

namespace Application.Translators.AiSettings;

public class AiSettingsCommandsToEventTranslator : IAiSettingsCommandsToEventTranslator
{
    public AiSettingsEvent ToEvent(CreateAiSettingsCommand command)
    {
        var domainEvent = new AiSettingsCreatedEvent(command.AiSettingsId, command.Prompt,
            command.LanguageModelPath);
        return CreateDatabaseEvent(nameof(AiSettingsCreatedEvent), command.AiSettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    public AiSettingsEvent ToEvent(ChangePromptCommand command)
    {
        var domainEvent = new PromptChangedEvent(command.AiSettingsId, command.Prompt);
        return CreateDatabaseEvent(nameof(PromptChangedEvent), command.AiSettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    public AiSettingsEvent ToEvent(ChangeLanguageModelCommand command)
    {
        var domainEvent = new LanguageModelChangedEvent(command.AiSettingsId, command.LanguageModelPath);
        return CreateDatabaseEvent(nameof(LanguageModelChangedEvent), command.AiSettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static AiSettingsEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new AiSettingsEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName, entityId, json);
    }
}