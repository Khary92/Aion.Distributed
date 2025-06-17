
namespace Domain.Events.Tags;

public record TagUpdatedEvent(
    Guid TagId,
    string Name);