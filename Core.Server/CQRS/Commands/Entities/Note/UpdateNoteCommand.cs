
namespace Application.Contract.CQRS.Commands.Entities.Note;

public record UpdateNoteCommand(Guid NoteId, string Text, Guid NoteTypeId, Guid TimeSlotId);