namespace Domain.Events.Tags;

public record TagCreatedEvent(
    Guid TagId,
    string Name);