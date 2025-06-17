namespace Domain.Events.TimeSlots;

public record NoteAddedEvent(Guid TimeSlotId, Guid NoteId);