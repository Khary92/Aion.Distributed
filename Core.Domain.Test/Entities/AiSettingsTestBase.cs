using System.Text.Json;
using Domain.Entities;
using Domain.Events.AiSettings;

namespace Core.Domain.Test.Entities;

public class AiSettingsTestBase : AggregateTestBase<AiSettingsEvent>
{
    protected virtual AiSettings Rehydrate(List<AiSettingsEvent> events)
    {
        return AiSettings.Rehydrate(events);
    }

    protected override AiSettingsEvent WrapEvent(object domainEvent)
    {
        return new AiSettingsEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertAiSettingsState(
        AiSettings aggregate,
        Guid expectedId,
        string expectedPath,
        string expectedPrompt)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.AiSettingsId, Is.EqualTo(expectedId));
            Assert.That(aggregate.LanguageModelPath, Is.EqualTo(expectedPath));
            Assert.That(aggregate.Prompt, Is.EqualTo(expectedPrompt));
        });
    }
}