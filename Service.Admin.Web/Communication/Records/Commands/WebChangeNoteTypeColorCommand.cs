namespace Service.Admin.Web.Communication.Records.Commands;

public record WebChangeNoteTypeColorCommand(Guid NoteTypeId, string Color, Guid TraceId);