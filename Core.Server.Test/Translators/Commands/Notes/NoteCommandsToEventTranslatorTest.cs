using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.Note;
using Core.Server.Translators.Commands.Notes;
using Domain.Events.Note;

namespace Core.Server.Test.Translators.Commands.Notes;

[TestFixture]
[TestOf(typeof(NoteCommandsToEventTranslator))]
public class NoteCommandsToEventTranslatorTest
{
    private NoteCommandsToEventTranslator _translator;

    [SetUp]
    public void SetUp()
    {
        _translator = new NoteCommandsToEventTranslator();
    }

    [Test]
    public void ToEvent_CreateNoteCommand_BuildsExpectedNoteEvent()
    {
        var noteId = Guid.NewGuid();
        var noteTypeId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();
        var timeSlotId = Guid.NewGuid();
        var timestamp = DateTimeOffset.UtcNow;
        var cmd = new CreateNoteCommand(noteId, "text", noteTypeId, ticketId, timeSlotId, timestamp, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(noteId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(NoteCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<NoteCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimeSlotId, Is.EqualTo(timeSlotId));
        Assert.That(payload.NoteId, Is.EqualTo(noteId));
        Assert.That(payload.NoteTypeId, Is.EqualTo(noteTypeId));
        Assert.That(payload.TicketId, Is.EqualTo(ticketId));
        Assert.That(payload.Text, Is.EqualTo("text"));
    }

    [Test]
    public void ToEvent_UpdateNoteCommand_BuildsExpectedNoteEvent()
    {
        var noteId = Guid.NewGuid();
        var noteTypeId = Guid.NewGuid();
        var cmd = new UpdateNoteCommand(noteId, "updated", noteTypeId, Guid.NewGuid(), Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(noteId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(NoteUpdatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<NoteUpdatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.NoteId, Is.EqualTo(noteId));
        Assert.That(payload.NoteTypeId, Is.EqualTo(noteTypeId));
        Assert.That(payload.Text, Is.EqualTo("updated"));
    }
}