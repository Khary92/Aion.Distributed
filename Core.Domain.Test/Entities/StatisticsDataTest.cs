using Domain.Entities;
using Domain.Events.StatisticsData;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(StatisticsData))]
public class StatisticsDataTest : StatisticsDataTestBase
{
    private readonly Guid _initialStatisticsId = Guid.NewGuid();
    private readonly bool _initialIsProductive = false;
    private readonly bool _initialIsNeutral = false;
    private readonly bool _initialIsUnproductive = false;
    private readonly List<Guid> _initialTagIds = [];
    private readonly Guid _initialTimeSlotId = Guid.NewGuid();


    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new StatisticsDataCreatedEvent(_initialStatisticsId, _initialIsProductive, _initialIsNeutral,
            _initialIsUnproductive, _initialTagIds, _initialTimeSlotId);

        var events = CreateEventList(created);
        var aggregate = Rehydrate(events);
        AssertStatisticsDataState(aggregate, _initialStatisticsId, _initialIsProductive, _initialIsNeutral,
            _initialIsUnproductive, _initialTagIds, _initialTimeSlotId);
    }

    [Test]
    public void ProductivityChangedEventChangesFields()
    {
        var newProductiveState = true;
        var newNeutralState = true;
        var newUnproductiveState = true;

        var created = new StatisticsDataCreatedEvent(_initialStatisticsId, _initialIsProductive, _initialIsNeutral,
            _initialIsUnproductive, _initialTagIds, _initialTimeSlotId);
        var productivityChanged =
            new ProductivityChangedEvent(Guid.NewGuid(), newProductiveState, newNeutralState, newUnproductiveState);

        var events = CreateEventList(created, productivityChanged);
        var aggregate = Rehydrate(events);

        AssertStatisticsDataState(aggregate, _initialStatisticsId, newProductiveState, newNeutralState,
            newUnproductiveState, _initialTagIds, _initialTimeSlotId);
    }

    [Test]
    public void TagSelectionChangedEventChangesFields()
    {
        var newTags = new List<Guid> { Guid.NewGuid() };

        var created = new StatisticsDataCreatedEvent(_initialStatisticsId, _initialIsProductive, _initialIsNeutral,
            _initialIsUnproductive, _initialTagIds, _initialTimeSlotId);
        var tagSelectionChanged =
            new TagSelectionChangedEvent(Guid.NewGuid(), newTags);

        var events = CreateEventList(created, tagSelectionChanged);
        var aggregate = Rehydrate(events);

        AssertStatisticsDataState(aggregate, _initialStatisticsId, _initialIsProductive, _initialIsNeutral,
            _initialIsUnproductive, newTags, _initialTimeSlotId);
    }
}