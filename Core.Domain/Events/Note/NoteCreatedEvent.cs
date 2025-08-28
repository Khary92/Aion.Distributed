namespace Domain.Events.Note;

public record NoteCreatedEvent(
    Guid NoteId,
    string Text,
    Guid NoteTypeId,
    Guid TicketId,
    Guid TimeSlotId,
    DateTimeOffset TimeStamp);