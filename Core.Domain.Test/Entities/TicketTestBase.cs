using System.Text.Json;
using Domain.Entities;
using Domain.Events.Tickets;

namespace Core.Domain.Test.Entities;

public abstract class TicketTestBase : AggregateTestBase<TicketEvent>
{
    protected virtual Ticket Rehydrate(List<TicketEvent> events)
    {
        return Ticket.Rehydrate(events);
    }

    protected override TicketEvent WrapEvent(object domainEvent)
    {
        return new TicketEvent(
            Guid.NewGuid(),
            new DateTime(2023, 1, 1, 12, 0, 0),
            new TimeSpan(0),
            domainEvent.GetType().Name,
            Guid.NewGuid(),
            JsonSerializer.Serialize(domainEvent));
    }

    protected static void AssertTicketState(
        Ticket aggregate,
        Guid ticketId,
        string name,
        string bookingNumber,
        List<Guid> sprintIds,
        string documentation)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregate.TicketId, Is.EqualTo(ticketId));
            Assert.That(aggregate.Name, Is.EqualTo(name));
            Assert.That(aggregate.BookingNumber, Is.EqualTo(bookingNumber));
            Assert.That(aggregate.SprintIds, Is.EqualTo(sprintIds));
            Assert.That(aggregate.Documentation, Is.EqualTo(documentation));
        });
    }
}