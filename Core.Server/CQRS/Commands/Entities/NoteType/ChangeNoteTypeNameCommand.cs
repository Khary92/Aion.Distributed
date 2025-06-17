
namespace Application.Contract.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeNameCommand(Guid NoteTypeId, string Name);