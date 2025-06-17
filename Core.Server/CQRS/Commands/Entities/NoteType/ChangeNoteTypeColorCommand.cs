
namespace Application.Contract.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeColorCommand(Guid NoteTypeId, string Color);