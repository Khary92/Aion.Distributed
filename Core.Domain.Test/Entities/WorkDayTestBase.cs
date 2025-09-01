using System.Text.Json;
using Domain.Entities;
using Domain.Events.WorkDays;

namespace Core.Domain.Test.Entities;

public class WorkDayTestBase : AggregateTestBase<WorkDayEvent>
{
    protected static WorkDay Rehydrate(List<WorkDayEvent> events)
    {
        return WorkDay.Rehydrate(events);
    }

    protected override WorkDayEvent WrapEvent(object domainEvent)
    {
        return new WorkDayEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertWorkDayState(
        WorkDay aggregate,
        Guid expectedId,
        DateTimeOffset expectedDate)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.WorkDayId, Is.EqualTo(expectedId));
            Assert.That(aggregate.Date, Is.EqualTo(expectedDate));
        });
    }
}