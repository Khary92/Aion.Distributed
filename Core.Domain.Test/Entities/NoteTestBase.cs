using System.Text.Json;
using Domain.Entities;
using Domain.Events.Note;

namespace Core.Domain.Test.Entities;

public class NoteTestBase : AggregateTestBase<NoteEvent>
{
    protected static Note Rehydrate(List<NoteEvent> events)
    {
        return Note.Rehydrate(events);
    }

    protected override NoteEvent WrapEvent(object domainEvent)
    {
        return new NoteEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertNoteState(
        Note aggregate,
        Guid noteId,
        string expectedText,
        Guid expectedNoteTypeId,
        Guid expectedTimeSlotId,
        DateTimeOffset expectedTimeStamp)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.NoteId, Is.EqualTo(noteId));
            Assert.That(aggregate.NoteTypeId, Is.EqualTo(expectedNoteTypeId));
            Assert.That(aggregate.Text, Is.EqualTo(expectedText));
            Assert.That(aggregate.TicketId, Is.EqualTo(expectedTimeSlotId));
            Assert.That(aggregate.TimeStamp, Is.EqualTo(expectedTimeStamp));
        });
    }
}