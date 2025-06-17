using Domain.Events.Tickets;

namespace Domain.Interfaces;

public interface ITicketEventsStore : IEventStore<TicketEvent>
{
    Task<List<TicketDocumentationChangedEvent>> GetTicketDocumentationEventsByTicketId(Guid ticketId);
}