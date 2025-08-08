namespace Service.Admin.Web.Communication.NoteType.Records;

public record WebChangeNoteTypeNameCommand(Guid NoteTypeId, string Name, Guid TraceId);