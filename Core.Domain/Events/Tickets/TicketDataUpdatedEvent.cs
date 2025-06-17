using System.Collections.ObjectModel;

namespace Domain.Events.Tickets;

public record TicketDataUpdatedEvent(
    Guid TicketId,
    string Name,
    string BookingNumber,
    Collection<Guid> SprintIds);