using System.Text.Json;
using Domain.Entities;
using Domain.Events.Tags;

namespace Core.Domain.Test.Entities;

public class TagTestBase : AggregateTestBase<TagEvent>
{
    protected static Tag Rehydrate(List<TagEvent> events)
    {
        return Tag.Rehydrate(events);
    }

    protected override TagEvent WrapEvent(object domainEvent)
    {
        return new TagEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertTagState(
        Tag aggregate,
        Guid expectedId,
        string expectedName)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.TagId, Is.EqualTo(expectedId));
            Assert.That(aggregate.Name, Is.EqualTo(expectedName));
        });
    }
}