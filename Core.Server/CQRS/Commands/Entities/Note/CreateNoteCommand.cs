
namespace Application.Contract.CQRS.Commands.Entities.Note;

public record CreateNoteCommand(Guid NoteId, string Text, Guid NoteTypeId, Guid TimeSlotId, DateTimeOffset TimeStamp);    