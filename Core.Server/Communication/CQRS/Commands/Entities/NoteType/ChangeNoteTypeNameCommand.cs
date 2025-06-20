namespace Core.Server.Communication.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeNameCommand(Guid NoteTypeId, string Name);