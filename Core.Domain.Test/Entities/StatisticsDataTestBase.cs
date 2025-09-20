using System.Text.Json;
using Domain.Entities;
using Domain.Events.StatisticsData;

namespace Core.Domain.Test.Entities;

public abstract class StatisticsDataTestBase : AggregateTestBase<StatisticsDataEvent>
{
    protected static StatisticsData Rehydrate(List<StatisticsDataEvent> events)
    {
        return StatisticsData.Rehydrate(events);
    }

    protected override StatisticsDataEvent WrapEvent(object domainEvent)
    {
        return new StatisticsDataEvent(
            Guid.NewGuid(),
            DateTimeOffset.Now, 
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertStatisticsDataState(
        StatisticsData aggregate,
        Guid expectedId,
        bool isProductive,
        bool isNeutral,
        bool isUnproductive,
        List<Guid> tagIds,
        Guid timeSlotId)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.StatisticsId, Is.EqualTo(expectedId));
            Assert.That(aggregate.IsProductive, Is.EqualTo(isProductive));
            Assert.That(aggregate.IsNeutral, Is.EqualTo(isNeutral));
            Assert.That(aggregate.IsUnproductive, Is.EqualTo(isUnproductive));
            Assert.That(aggregate.TagIds, Is.EqualTo(tagIds));
            Assert.That(aggregate.TimeSlotId, Is.EqualTo(timeSlotId));
        });
    }
}