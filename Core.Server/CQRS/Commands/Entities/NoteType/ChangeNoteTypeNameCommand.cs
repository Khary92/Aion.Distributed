
namespace Service.Server.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeNameCommand(Guid NoteTypeId, string Name);