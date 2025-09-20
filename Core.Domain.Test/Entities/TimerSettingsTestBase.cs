using System.Text.Json;
using Domain.Entities;
using Domain.Events.TimerSettings;

namespace Core.Domain.Test.Entities;

public abstract class TimerSettingsTestBase : AggregateTestBase<TimerSettingsEvent>
{
    protected static TimerSettings Rehydrate(List<TimerSettingsEvent> events)
    {
        return TimerSettings.Rehydrate(events);
    }

    protected override TimerSettingsEvent WrapEvent(object domainEvent)
    {
        return new TimerSettingsEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertTimerSettingsState(
        TimerSettings aggregate,
        Guid timerSettingsId,
        int documentationSaveInterval,
        int snapshotSaveInterval)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.TimerSettingsId, Is.EqualTo(timerSettingsId));
            Assert.That(aggregate.DocumentationSaveInterval, Is.EqualTo(documentationSaveInterval));
            Assert.That(aggregate.SnapshotSaveInterval, Is.EqualTo(snapshotSaveInterval));
        });
    }
}