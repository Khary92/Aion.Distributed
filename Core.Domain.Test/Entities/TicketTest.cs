using Domain.Entities;
using Domain.Events.Tickets;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(Ticket))]
public class TicketTest : TicketTestBase
{
    private readonly Guid _initialTicketId = Guid.NewGuid();
    private const string InitialName = "initialName";
    private const string InitialBookingNumber = "initialBookingNumber";
    private const string InitialDocumentation = "initialDocumentation";
    private readonly List<Guid> _initialSprintIds = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new TicketCreatedEvent(_initialTicketId, InitialName, InitialBookingNumber,
            InitialDocumentation, _initialSprintIds);
        var events = CreateEventList(created);

        var aggregate = Rehydrate(events);

        AssertTicketState(aggregate, _initialTicketId, InitialName, InitialBookingNumber, _initialSprintIds,
            InitialDocumentation);
    }

    [Test]
    public void DataUpdatedEventChangesFields()
    {
        const string newName = "NewName";
        const string newBookingNumber = "NewBookingNumber";
        var newSprintIds = new List<Guid>();

        var created = new TicketCreatedEvent(_initialTicketId, InitialName, InitialBookingNumber,
            InitialDocumentation, _initialSprintIds);
        var updated = new TicketDataUpdatedEvent(Guid.NewGuid(), newName, newBookingNumber, newSprintIds);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertTicketState(aggregate, _initialTicketId, newName, newBookingNumber, newSprintIds, InitialDocumentation);
    }

    [Test]
    public void DocumentationChangedEventChangesField()
    {
        var newDocumentation = "newDocumentation";

        var created = new TicketCreatedEvent(_initialTicketId, InitialName, InitialBookingNumber,
            InitialDocumentation, _initialSprintIds);
        var updated = new TicketDocumentationChangedEvent(Guid.NewGuid(), newDocumentation);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertTicketState(aggregate, _initialTicketId, InitialName, InitialBookingNumber, _initialSprintIds,
            newDocumentation);
    }
}