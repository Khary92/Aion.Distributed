namespace Service.Admin.Web.Communication.Records.Commands;

public record WebChangeNoteTypeNameCommand(Guid NoteTypeId, string Name, Guid TraceId);