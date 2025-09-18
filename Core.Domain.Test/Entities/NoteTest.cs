using Domain.Entities;
using Domain.Events.Note;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(Note))]
public class NoteTest : NoteTestBase
{
    private readonly Guid _noteId = Guid.NewGuid();
    private readonly Guid _initialNoteTypeId = Guid.NewGuid();
    private const string InitialText = "Initial text";
    private readonly Guid _initialTicketId = Guid.NewGuid();
    private readonly Guid _initialTimeSlotId = Guid.NewGuid();
    private readonly DateTimeOffset _initialTimeStamp = DateTimeOffset.Now.AddDays(-3);

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new NoteCreatedEvent(_noteId, InitialText, _initialNoteTypeId, _initialTicketId,
            _initialTimeSlotId, _initialTimeStamp);
        var events = CreateEventList(created);

        var aggregate = Rehydrate(events);

        AssertNoteState(aggregate, _noteId, InitialText, _initialNoteTypeId, _initialTicketId, _initialTimeSlotId,
            _initialTimeStamp);
    }

    [Test]
    public void UpdatedEventChangesFields()
    {
        const string newText = "new text";
        var newNoteTypeId = Guid.NewGuid();

        var created = new NoteCreatedEvent(_noteId, InitialText, _initialNoteTypeId, _initialTicketId,
            _initialTimeSlotId, _initialTimeStamp);
        var updated = new NoteUpdatedEvent(_noteId, newText, newNoteTypeId);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertNoteState(aggregate, _noteId, newText, newNoteTypeId, _initialTicketId, _initialTimeSlotId,
            _initialTimeStamp);
    }
}