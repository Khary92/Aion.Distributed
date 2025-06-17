namespace Domain.Events.TimeSlots;

public record StartTimeSetEvent(Guid TimeSlotId, DateTimeOffset Time);