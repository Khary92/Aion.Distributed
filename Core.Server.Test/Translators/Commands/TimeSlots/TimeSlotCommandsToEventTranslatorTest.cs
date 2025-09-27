using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Core.Server.Translators.Commands.TimeSlots;
using Domain.Events.TimeSlots;

namespace Core.Server.Test.Translators.Commands.TimeSlots;

[TestFixture]
[TestOf(typeof(TimeSlotCommandsToEventTranslator))]
public class TimeSlotCommandsToEventTranslatorTest
{
    [SetUp]
    public void SetUp()
    {
        _translator = new TimeSlotCommandsToEventTranslator();
    }

    private TimeSlotCommandsToEventTranslator _translator;


    [Test]
    public void ToEvent_CreateTimeSlotCommand_BuildsExpectedEvent()
    {
        var timeSlotId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();
        var workDayId = Guid.NewGuid();
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(1);
        var cmd = new CreateTimeSlotCommand(
            timeSlotId,
            ticketId,
            workDayId,
            start,
            end,
            true,
            Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(timeSlotId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TimeSlotCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TimeSlotCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimeSlotId, Is.EqualTo(timeSlotId));
        Assert.That(payload.SelectedTicketId, Is.EqualTo(ticketId));
        Assert.That(payload.WorkDayId, Is.EqualTo(workDayId));
        Assert.That(payload.StartTime, Is.EqualTo(start));
        Assert.That(payload.EndTime, Is.EqualTo(end));
        Assert.That(payload.IsTimerRunning, Is.True);
    }

    [Test]
    public void ToEvent_AddNoteCommand_BuildsExpectedEvent()
    {
        var timeSlotId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var cmd = new AddNoteCommand(timeSlotId, noteId, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(timeSlotId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(NoteAddedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<NoteAddedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimeSlotId, Is.EqualTo(timeSlotId));
        Assert.That(payload.NoteId, Is.EqualTo(noteId));
    }

    [Test]
    public void ToEvent_SetStartTimeCommand_BuildsExpectedEvent()
    {
        var timeSlotId = Guid.NewGuid();
        var time = DateTimeOffset.UtcNow;
        var cmd = new SetStartTimeCommand(timeSlotId, time, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(timeSlotId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(StartTimeSetEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<StartTimeSetEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimeSlotId, Is.EqualTo(timeSlotId));
        Assert.That(payload.Time, Is.EqualTo(time));
    }

    [Test]
    public void ToEvent_SetEndTimeCommand_BuildsExpectedEvent()
    {
        var timeSlotId = Guid.NewGuid();
        var time = DateTimeOffset.UtcNow.AddMinutes(30);
        var cmd = new SetEndTimeCommand(timeSlotId, time, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(timeSlotId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(EndTimeSetEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<EndTimeSetEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimeSlotId, Is.EqualTo(timeSlotId));
        Assert.That(payload.Time, Is.EqualTo(time));
    }
}