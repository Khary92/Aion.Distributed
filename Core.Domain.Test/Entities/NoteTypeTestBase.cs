using System.Text.Json;
using Domain.Entities;
using Domain.Events.NoteTypes;

namespace Core.Domain.Test.Entities;

public class NoteTypeTestBase : AggregateTestBase<NoteTypeEvent>
{
    protected virtual NoteType Rehydrate(List<NoteTypeEvent> events)
    {
        return NoteType.Rehydrate(events);
    }

    protected override NoteTypeEvent WrapEvent(object domainEvent)
    {
        return new NoteTypeEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertNoteTypeState(
        NoteType aggregate,
        Guid expectedId,
        string expectedName,
        string expectedColor)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.NoteTypeId, Is.EqualTo(expectedId));
            Assert.That(aggregate.Name, Is.EqualTo(expectedName));
            Assert.That(aggregate.Color, Is.EqualTo(expectedColor));
        });
    }
}