namespace Core.Server.Communication.Records.Commands.Entities.Note;

public record CreateNoteCommand(
    Guid NoteId,
    string Text,
    Guid NoteTypeId,
    Guid TicketId,
    Guid TimeSlotId,
    DateTimeOffset TimeStamp,
    Guid TraceId);