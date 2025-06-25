using Domain.Entities;
using Domain.Events.TimeSlots;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(TimeSlot))]
public class TimeSlotTest : TimeSlotTestBase
{
    private readonly Guid _initialId = Guid.NewGuid();
    private readonly Guid _initialTicketId = Guid.NewGuid();
    private readonly Guid _initialWorkDayId = Guid.NewGuid();
    private readonly DateTimeOffset _initialStartTime = DateTimeOffset.Now.AddHours(-1);
    private readonly DateTimeOffset _initialEndTime = DateTimeOffset.Now.AddHours(1);
    private const bool InitialIsTimerRunning = true;
    private readonly List<Guid> _initialNoteIds = [];

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new TimeSlotCreatedEvent(_initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);
        var events = CreateEventList(created);
        var aggregate = Rehydrate(events);

        AssertTimeSlotState(aggregate, _initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);
    }

    [Test]
    public void StartTimeSetEventChangesField()
    {
        var newStartTime = DateTimeOffset.Now.AddHours(-5);

        var created = new TimeSlotCreatedEvent(_initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);

        var updated = new StartTimeSetEvent(Guid.NewGuid(), newStartTime);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertTimeSlotState(aggregate, _initialId, _initialTicketId, _initialWorkDayId, newStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);
    }

    [Test]
    public void EndTimeSetEventChangesField()
    {
        var newEndTime = DateTimeOffset.Now.AddHours(+5);

        var created = new TimeSlotCreatedEvent(_initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);

        var updated = new EndTimeSetEvent(Guid.NewGuid(), newEndTime);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertTimeSlotState(aggregate, _initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            newEndTime, InitialIsTimerRunning, _initialNoteIds);
    }

    [Test]
    public void NoteAddedEventAppendsField()
    {
        var newNoteId = Guid.NewGuid();

        var created = new TimeSlotCreatedEvent(_initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);

        var added = new NoteAddedEvent(Guid.NewGuid(), newNoteId);
        var events = CreateEventList(created, added);

        var aggregate = Rehydrate(events);

        AssertTimeSlotState(aggregate, _initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, [newNoteId]);
    }

    [Test]
    public void GetDurationInMinutes()
    {
        var created = new TimeSlotCreatedEvent(_initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);
        var events = CreateEventList(created);

        var aggregate = Rehydrate(events);

        Assert.That(aggregate.GetDurationInMinutes(), Is.EqualTo(120));
    }

    [Test]
    public void GetDurationInSeconds()
    {
        var created = new TimeSlotCreatedEvent(_initialId, _initialTicketId, _initialWorkDayId, _initialStartTime,
            _initialEndTime, InitialIsTimerRunning, _initialNoteIds);
        var events = CreateEventList(created);

        var aggregate = Rehydrate(events);

        Assert.That(aggregate.GetDurationInSeconds(), Is.EqualTo(7200));
    }
}