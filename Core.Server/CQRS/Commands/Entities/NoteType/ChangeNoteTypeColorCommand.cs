
namespace Service.Server.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeColorCommand(Guid NoteTypeId, string Color);