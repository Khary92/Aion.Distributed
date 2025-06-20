
namespace Service.Server.Communication.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeColorCommand(Guid NoteTypeId, string Color);