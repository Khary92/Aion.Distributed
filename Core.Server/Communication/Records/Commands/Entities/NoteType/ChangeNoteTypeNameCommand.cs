namespace Core.Server.Communication.Records.Commands.Entities.NoteType;

public record ChangeNoteTypeNameCommand(Guid NoteTypeId, string Name);