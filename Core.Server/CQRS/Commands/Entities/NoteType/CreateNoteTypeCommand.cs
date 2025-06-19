
namespace Service.Server.CQRS.Commands.Entities.NoteType;

public record CreateNoteTypeCommand(Guid NoteTypeId, string Name, string Color);