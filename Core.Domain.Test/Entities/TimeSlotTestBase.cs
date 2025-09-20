using System.Text.Json;
using Domain.Entities;
using Domain.Events.TimeSlots;

namespace Core.Domain.Test.Entities;

public class TimeSlotTestBase : AggregateTestBase<TimeSlotEvent>
{
    protected static TimeSlot Rehydrate(List<TimeSlotEvent> events)
    {
        return TimeSlot.Rehydrate(events);
    }

    protected override TimeSlotEvent WrapEvent(object domainEvent)
    {
        return new TimeSlotEvent(
            Guid.NewGuid(),
            DateTimeOffset.Now, 
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertTimeSlotState(
        TimeSlot aggregate,
        Guid expectedId,
        Guid expectedTicketId,
        Guid expectedWorkDayId,
        DateTimeOffset expectedStartTime,
        DateTimeOffset expectedEndTime,
        bool expectedIsTimerRunning,
        List<Guid> expectedNoteIds)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.TimeSlotId, Is.EqualTo(expectedId));
            Assert.That(aggregate.SelectedTicketId, Is.EqualTo(expectedTicketId));
            Assert.That(aggregate.WorkDayId, Is.EqualTo(expectedWorkDayId));
            Assert.That(aggregate.StartTime, Is.EqualTo(expectedStartTime));
            Assert.That(aggregate.EndTime, Is.EqualTo(expectedEndTime));
            Assert.That(aggregate.IsTimerRunning, Is.EqualTo(expectedIsTimerRunning));
            Assert.That(aggregate.NoteIds, Is.EqualTo(expectedNoteIds));
        });
    }
}