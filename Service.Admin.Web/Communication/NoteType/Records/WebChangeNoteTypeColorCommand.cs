namespace Service.Admin.Web.Communication.NoteType.Records;

public record WebChangeNoteTypeColorCommand(Guid NoteTypeId, string Color, Guid TraceId);