using Proto.Command.NoteTypes;

namespace Service.Admin.Web.Communication.NoteType.Records;

public record WebCreateNoteTypeCommand(Guid NoteTypeId, string Name, string Color, Guid TraceId);