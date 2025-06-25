using System.Text.Json;
using Domain.Entities;
using Domain.Events.Settings;

namespace Core.Domain.Test.Entities;

public abstract class SettingsTestBase : AggregateTestBase<SettingsEvent>
{
    protected virtual Settings Rehydrate(List<SettingsEvent> events)
    {
        return Settings.Rehydrate(events);
    }

    protected override SettingsEvent WrapEvent(object domainEvent)
    {
        return new SettingsEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertSettingsState(
        Settings aggregate,
        Guid expectedId,
        string expectedPath,
        bool expectedAddToSprint)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.SettingsId, Is.EqualTo(expectedId));
            Assert.That(aggregate.ExportPath, Is.EqualTo(expectedPath));
            Assert.That(aggregate.IsAddNewTicketsToCurrentSprintActive, Is.EqualTo(expectedAddToSprint));
        });
    }
}