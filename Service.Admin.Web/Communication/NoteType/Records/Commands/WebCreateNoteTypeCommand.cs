namespace Service.Admin.Web.Communication.NoteType.Records.Commands;

public record WebCreateNoteTypeCommand(Guid NoteTypeId, string Name, string Color, Guid TraceId);