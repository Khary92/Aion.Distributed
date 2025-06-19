namespace Domain.Events.Tickets;

public record TicketDataUpdatedEvent(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds);