namespace Service.Admin.Web.Communication.NoteType.Records.Commands;

public record WebChangeNoteTypeNameCommand(Guid NoteTypeId, string Name, Guid TraceId);