namespace Service.Admin.Web.Communication.NoteType.Records.Commands;

public record WebChangeNoteTypeColorCommand(Guid NoteTypeId, string Color, Guid TraceId);