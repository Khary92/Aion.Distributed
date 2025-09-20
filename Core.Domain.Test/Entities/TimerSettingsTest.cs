using Domain.Entities;
using Domain.Events.TimerSettings;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(TimerSettings))]
public class TimerSettingsTest : TimerSettingsTestBase
{
    private readonly Guid _initialTimerSettingsId = Guid.NewGuid();
    private readonly int _documentationSaveInterval = 10;
    private readonly int _snapshotSaveInterval = 10;

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created =
            new TimerSettingsCreatedEvent(_initialTimerSettingsId, _documentationSaveInterval, _snapshotSaveInterval);
        var events = CreateEventList(created);

        var aggregate = Rehydrate(events);

        AssertTimerSettingsState(aggregate, _initialTimerSettingsId, _documentationSaveInterval, _snapshotSaveInterval);
    }

    [Test]
    public void DocuIntervalChangedEventChangesField()
    {
        var newSaveInterval = 42;
        var created =
            new TimerSettingsCreatedEvent(_initialTimerSettingsId, _documentationSaveInterval, _snapshotSaveInterval);
        var productivityChanged = new DocuIntervalChangedEvent(Guid.NewGuid(), newSaveInterval);
        var events = CreateEventList(created, productivityChanged);

        var aggregate = Rehydrate(events);

        AssertTimerSettingsState(aggregate, _initialTimerSettingsId, newSaveInterval, _snapshotSaveInterval);
    }

    [Test]
    public void SnapshotIntervalChangedEventChangesField()
    {
        var newSaveInterval = 42;
        var created =
            new TimerSettingsCreatedEvent(_initialTimerSettingsId, _documentationSaveInterval, _snapshotSaveInterval);
        var productivityChanged = new SnapshotIntervalChangedEvent(Guid.NewGuid(), newSaveInterval);
        var events = CreateEventList(created, productivityChanged);

        var aggregate = Rehydrate(events);

        AssertTimerSettingsState(aggregate, _initialTimerSettingsId, _documentationSaveInterval, newSaveInterval);
    }
}