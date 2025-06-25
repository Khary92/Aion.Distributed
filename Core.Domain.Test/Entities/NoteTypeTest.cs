using Domain.Entities;
using Domain.Events.NoteTypes;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(NoteType))]
public class NoteTypeTest : NoteTypeTestBase
{
    private static readonly Guid InitialId = Guid.NewGuid();
    private const string InitialName = "A name";
    private const string InitialColor = "#FFFFF";

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new NoteTypeCreatedEvent(InitialId, InitialName, InitialColor);
        var events = CreateEventList(created);

        var aggregate = Rehydrate(events);

        AssertNoteTypeState(aggregate, InitialId, InitialName, InitialColor);
    }

    [Test]
    public void NoteTypeNameChangedEventChangesFields()
    {
        const string newName = "newName";

        var created = new NoteTypeCreatedEvent(InitialId, InitialName, InitialColor);
        var updated = new NoteTypeNameChangedEvent(InitialId, newName);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertNoteTypeState(aggregate, InitialId, newName, InitialColor);
    }

    [Test]
    public void NoteTypeColorChangedEventChangesFields()
    {
        const string newColor = "#000000";

        var created = new NoteTypeCreatedEvent(InitialId, InitialName, InitialColor);
        var updated = new NoteTypeColorChangedEvent(InitialId, newColor);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertNoteTypeState(aggregate, InitialId, InitialName, newColor);
    }
}