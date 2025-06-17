namespace Domain.Events.TimeSlots;

public record EndTimeSetEvent(Guid TimeSlotId, DateTimeOffset Time);