using Domain.Entities;
using Domain.Events.Sprints;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(Sprint))]
public class SprintTest : SprintTestBase
{
    private const string InitialName = "InitialName";
    private const bool InitialIsActive = true;

    private readonly Guid _initialSprintId = Guid.NewGuid();
    private readonly DateTimeOffset _initialStartDate = DateTimeOffset.Now.AddDays(-5);
    private readonly DateTimeOffset _initialEndDate = DateTimeOffset.Now.AddDays(-3);
    private readonly List<Guid> _initialTicketIds = [];

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new SprintCreatedEvent(_initialSprintId, InitialName, _initialStartDate, _initialEndDate,
            InitialIsActive, _initialTicketIds);
        var events = CreateEventList(created);
        var aggregate = Rehydrate(events);
        AssertSprintState(aggregate, _initialSprintId, InitialName, _initialStartDate, _initialEndDate,
            InitialIsActive, _initialTicketIds);
    }

    [Test]
    public void DataUpdatedEventChangesFields()
    {
        const string newName = "NewName";
        var newStartDate = DateTimeOffset.Now.AddDays(+2);
        var newEndDate = DateTimeOffset.Now.AddDays(+5);

        var created = new SprintCreatedEvent(_initialSprintId, InitialName, _initialStartDate, _initialEndDate,
            InitialIsActive, _initialTicketIds);
        var updated = new SprintDataUpdatedEvent(Guid.NewGuid(), newName, newStartDate, newEndDate);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertSprintState(aggregate, _initialSprintId, newName, newStartDate, newEndDate, InitialIsActive,
            _initialTicketIds);
    }

    [Test]
    public void ActiveStatusChangedEventChangesField()
    {
        var created = new SprintCreatedEvent(_initialSprintId, InitialName, _initialStartDate, _initialEndDate,
            InitialIsActive, _initialTicketIds);
        var updated = new SprintActiveStatusChangedEvent(Guid.NewGuid(), false);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertSprintState(aggregate, _initialSprintId, InitialName, _initialStartDate, _initialEndDate, false,
            _initialTicketIds);
    }

    [Test]
    public void TicketAddedToSprintEventChangesField()
    {
        var newTicketId = Guid.NewGuid();

        var created = new SprintCreatedEvent(_initialSprintId, InitialName, _initialStartDate, _initialEndDate,
            InitialIsActive, _initialTicketIds);
        var updated = new TicketAddedToSprintEvent(Guid.NewGuid(), newTicketId);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertSprintState(aggregate, _initialSprintId, InitialName, _initialStartDate, _initialEndDate,
            InitialIsActive, [newTicketId]);

        var anotherNewTicketId = Guid.NewGuid();
        var anotherUpdate = new TicketAddedToSprintEvent(Guid.NewGuid(), anotherNewTicketId);
        events = CreateEventList(created, updated, anotherUpdate);

        aggregate = Rehydrate(events);

        AssertSprintState(aggregate, _initialSprintId, InitialName, _initialStartDate, _initialEndDate,
            InitialIsActive,
            [newTicketId, anotherNewTicketId]);
    }
}