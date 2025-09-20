using System.Text.Json;
using Domain.Entities;
using Domain.Events.Sprints;

namespace Core.Domain.Test.Entities;

public abstract class SprintTestBase : AggregateTestBase<SprintEvent>
{
    protected static Sprint Rehydrate(List<SprintEvent> events)
    {
        return Sprint.Rehydrate(events);
    }

    protected override SprintEvent WrapEvent(object domainEvent)
    {
        return new SprintEvent(
            Guid.NewGuid(),
            DateTimeOffset.Now, 
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertSprintState(
        Sprint aggregate,
        Guid expectedId,
        string expectedName,
        DateTimeOffset expectedStartDate,
        DateTimeOffset expectedEndDate,
        bool expectedIsActive,
        List<Guid> expectedTicketIds)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.SprintId, Is.EqualTo(expectedId));
            Assert.That(aggregate.Name, Is.EqualTo(expectedName));
            Assert.That(aggregate.StartDate, Is.EqualTo(expectedStartDate));
            Assert.That(aggregate.EndDate, Is.EqualTo(expectedEndDate));
            Assert.That(aggregate.IsActive, Is.EqualTo(expectedIsActive));
            Assert.That(aggregate.TicketIds, Is.EqualTo(expectedTicketIds));
        });
    }
}