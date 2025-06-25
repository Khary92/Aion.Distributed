using Domain.Entities;
using Domain.Events.Tags;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(Tag))]
public class TagTest : TagTestBase
{
    private readonly Guid _initialId = Guid.NewGuid();
    private const string InitialName = "initial name";

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new TagCreatedEvent(_initialId, InitialName);
        var events = CreateEventList(created);
        var aggregate = Rehydrate(events);
        AssertTagState(aggregate, _initialId, InitialName);
    }

    [Test]
    public void DataUpdatedEventChangesFields()
    {
        const string newName = "NewName";

        var created = new TagCreatedEvent(_initialId, InitialName);
        var updated = new TagUpdatedEvent(Guid.NewGuid(), newName);

        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertTagState(aggregate, _initialId, newName);
    }
}