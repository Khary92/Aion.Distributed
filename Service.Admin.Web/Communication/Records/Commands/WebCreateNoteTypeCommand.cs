namespace Service.Admin.Web.Communication.Records.Commands;

public record WebCreateNoteTypeCommand(Guid NoteTypeId, string Name, string Color, Guid TraceId);