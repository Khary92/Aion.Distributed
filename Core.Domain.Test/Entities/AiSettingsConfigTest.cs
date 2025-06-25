using Domain.Entities;
using Domain.Events.AiSettings;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(AiSettings))]
public class AiSettingsConfigTest : AiSettingsTestBase
{
    private static readonly Guid InitialId = Guid.NewGuid();
    private const string InitialExportPath = "Export A";
    private const string InitialPrompt = "Prompt A";

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new AiSettingsCreatedEvent(InitialId, InitialPrompt, InitialExportPath);
        var events = CreateEventList(created);
        var aggregate = Rehydrate(events);
        AssertAiSettingsState(aggregate, InitialId, InitialExportPath, InitialPrompt);
    }

    [Test]
    public void PromptChangedEventChangesFields()
    {
        const string newPrompt = "another prompt";

        var created = new AiSettingsCreatedEvent(InitialId, InitialPrompt, InitialExportPath);
        var updated = new PromptChangedEvent(InitialId, newPrompt);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertAiSettingsState(aggregate, InitialId, InitialExportPath, newPrompt);
    }

    [Test]
    public void LanguageModelChangedEventChangesFields()
    {
        const string newPath = "another path";

        var created = new AiSettingsCreatedEvent(InitialId, InitialPrompt, InitialExportPath);
        var updated = new LanguageModelChangedEvent(InitialId, newPath);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertAiSettingsState(aggregate, InitialId, newPath, InitialPrompt);
    }
}