namespace Domain.Events.Note;

public record NoteUpdatedEvent(
    Guid NoteId,
    string Text,
    Guid NoteTypeId);