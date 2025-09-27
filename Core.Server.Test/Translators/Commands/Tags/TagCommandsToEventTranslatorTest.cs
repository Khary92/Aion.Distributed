using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.Tags;
using Core.Server.Translators.Commands.Tags;
using Domain.Events.Tags;

namespace Core.Server.Test.Translators.Commands.Tags;

[TestFixture]
[TestOf(typeof(TagCommandsToEventTranslator))]
public class TagCommandsToEventTranslatorTest
{
    [SetUp]
    public void SetUp()
    {
        _translator = new TagCommandsToEventTranslator();
    }

    private TagCommandsToEventTranslator _translator;

    [Test]
    public void ToEvent_CreateTagCommand_BuildsExpectedTagEvent()
    {
        var tagId = Guid.NewGuid();
        var cmd = new CreateTagCommand(tagId, "Tag A", Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(tagId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TagCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TagCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TagId, Is.EqualTo(tagId));
        Assert.That(payload.Name, Is.EqualTo("Tag A"));
    }

    [Test]
    public void ToEvent_UpdateTagCommand_BuildsExpectedTagEvent()
    {
        var tagId = Guid.NewGuid();
        var cmd = new UpdateTagCommand(tagId, "Tag B", Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(tagId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TagUpdatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TagUpdatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TagId, Is.EqualTo(tagId));
        Assert.That(payload.Name, Is.EqualTo("Tag B"));
    }
}