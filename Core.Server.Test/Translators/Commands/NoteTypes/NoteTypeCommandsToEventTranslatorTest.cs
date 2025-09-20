using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.NoteType;
using Core.Server.Translators.Commands.NoteTypes;
using Domain.Events.NoteTypes;

namespace Core.Server.Test.Translators.Commands.NoteTypes;

[TestFixture]
[TestOf(typeof(NoteTypeCommandsToEventTranslator))]
public class NoteTypeCommandsToEventTranslatorTest
{
    private NoteTypeCommandsToEventTranslator _translator;

    [SetUp]
    public void SetUp()
    {
        _translator = new NoteTypeCommandsToEventTranslator();
    }

    [Test]
    public void ToEvent_CreateNoteTypeCommand_BuildsExpectedNoteTypeEvent()
    {
        var noteTypeId = Guid.NewGuid();
        var cmd = new CreateNoteTypeCommand(noteTypeId, "Bug", "#ff0000", Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(noteTypeId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(NoteTypeCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<NoteTypeCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.NoteTypeId, Is.EqualTo(noteTypeId));
        Assert.That(payload.Name, Is.EqualTo("Bug"));
        Assert.That(payload.Color, Is.EqualTo("#ff0000"));
    }

    [Test]
    public void ToEvent_ChangeNoteTypeNameCommand_BuildsExpectedNoteTypeEvent()
    {
        var noteTypeId = Guid.NewGuid();
        var cmd = new ChangeNoteTypeNameCommand(noteTypeId, "Feature", Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(noteTypeId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(NoteTypeNameChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<NoteTypeNameChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.NoteTypeId, Is.EqualTo(noteTypeId));
        Assert.That(payload.Name, Is.EqualTo("Feature"));
    }

    [Test]
    public void ToEvent_ChangeNoteTypeColorCommand_BuildsExpectedNoteTypeEvent()
    {
        var noteTypeId = Guid.NewGuid();
        var cmd = new ChangeNoteTypeColorCommand(noteTypeId, "#00ff00", Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(noteTypeId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(NoteTypeColorChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<NoteTypeColorChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.NoteTypeId, Is.EqualTo(noteTypeId));
        Assert.That(payload.Color, Is.EqualTo("#00ff00"));
    }
}