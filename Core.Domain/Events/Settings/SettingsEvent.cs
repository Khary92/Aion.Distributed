namespace Domain.Events.Settings;

public record SettingsEvent(
    Guid EventId,
    DateTime TimeStamp,
    TimeSpan Offset,
    string EventType,
    Guid EntityId,
    string EventPayload)
    : IDomainEvent;