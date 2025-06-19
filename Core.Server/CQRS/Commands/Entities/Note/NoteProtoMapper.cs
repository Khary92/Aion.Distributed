using Proto.Command.Notes;

namespace Service.Server.CQRS.Commands.Entities.Note;

public static class NoteProtoMapper
{
    public static CreateNoteCommand ToCommand(this CreateNoteCommandProto proto) =>
        new(Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId), Guid.Parse(proto.TimeSlotId),
            proto.TimeStamp.ToDateTimeOffset());

    public static UpdateNoteCommand ToCommand(this UpdateNoteCommandProto proto) =>
        new(Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId), Guid.Parse(proto.TimeSlotId));
}