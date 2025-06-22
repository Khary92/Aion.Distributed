namespace Core.Server.Communication.Records.Commands.Entities.NoteType;

public record ChangeNoteTypeColorCommand(Guid NoteTypeId, string Color);