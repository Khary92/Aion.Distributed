using System.Collections.ObjectModel;

namespace Domain.Events.Tickets;

public record TicketCreatedEvent(
    Guid TicketId,
    string Name,
    string BookingNumber,
    string Documentation,
    Collection<Guid> SprintIds);