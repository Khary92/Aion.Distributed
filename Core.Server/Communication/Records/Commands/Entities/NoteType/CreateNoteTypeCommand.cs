namespace Core.Server.Communication.Records.Commands.Entities.NoteType;

public record CreateNoteTypeCommand(Guid NoteTypeId, string Name, string Color, Guid TraceId);