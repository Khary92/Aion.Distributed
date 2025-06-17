namespace Domain.Events.Note;

public record NoteCreatedEvent(
    Guid NoteId,
    string Text,
    Guid NoteTypeId,
    Guid TimeSlotId,
    DateTimeOffset TimeStamp);