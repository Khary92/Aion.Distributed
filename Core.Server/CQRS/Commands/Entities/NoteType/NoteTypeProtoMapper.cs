using Proto.Command.NoteTypes;

namespace Service.Server.CQRS.Commands.Entities.NoteType;

public static class NoteTypeProtoMapper
{
    public static CreateNoteTypeCommand ToCommand(this CreateNoteTypeCommandProto proto) =>
        new(Guid.Parse(proto.NoteTypeId), proto.Name, proto.Color);

    public static ChangeNoteTypeNameCommand ToCommand(this ChangeNoteTypeNameCommandProto proto) =>
        new(Guid.Parse(proto.NoteTypeId), proto.Name);

    public static ChangeNoteTypeColorCommand ToCommand(this ChangeNoteTypeColorCommandProto proto) =>
        new(Guid.Parse(proto.NoteTypeId), proto.Color);
}