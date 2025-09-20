namespace Domain.Events.Tickets;

public record TicketEvent(
    Guid EventId,
    DateTimeOffset TimeStamp,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;