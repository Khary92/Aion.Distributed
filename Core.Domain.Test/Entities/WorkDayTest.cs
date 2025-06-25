using Domain.Entities;
using Domain.Events.WorkDays;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(WorkDay))]
public class WorkDayTest : WorkDayTestBase
{
    private readonly Guid _expectedId = Guid.NewGuid();
    private readonly DateTimeOffset _expectedDate = DateTimeOffset.UtcNow;

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new WorkDayCreatedEvent(_expectedId, _expectedDate);
        var events = CreateEventList(created);

        var aggregate = Rehydrate(events);

        AssertWorkDayState(aggregate, _expectedId, _expectedDate);
    }

    [Test]
    public void DataUpdatedEventChangesFields()
    {
        var newDate = DateTimeOffset.UtcNow.AddDays(1);

        var created = new WorkDayCreatedEvent(_expectedId, _expectedDate);
        var updated = new WorkDayUpdatedEvent(Guid.NewGuid(), newDate);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertWorkDayState(aggregate, _expectedId, newDate);
    }
}