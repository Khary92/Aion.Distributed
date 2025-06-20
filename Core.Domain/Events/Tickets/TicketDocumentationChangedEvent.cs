namespace Domain.Events.Tickets;

public record TicketDocumentationChangedEvent(Guid TicketId, string Documentation);