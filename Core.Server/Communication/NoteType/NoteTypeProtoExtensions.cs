using Proto.Command.NoteTypes;
using Proto.DTO.NoteType;
using Proto.Notifications.NoteType;
using Proto.Requests.NoteTypes;
using Service.Server.CQRS.Commands.Entities.NoteType;

namespace Service.Server.Communication.NoteType;

public static class NoteTypeProtoExtensions
{
    public static CreateNoteTypeCommand ToCommand(this CreateNoteTypeCommandProto proto) =>
        new(Guid.Parse(proto.NoteTypeId), proto.Name, proto.Color);

    public static NoteTypeNotification ToNotification(this CreateNoteTypeCommandProto proto) =>
        new()
        {
            NoteTypeCreated = new NoteTypeCreatedNotification
            {
                NoteTypeId = proto.NoteTypeId,
                Name = proto.Name,
                Color = proto.Color
            }
        };

    public static ChangeNoteTypeNameCommand ToCommand(this ChangeNoteTypeNameCommandProto proto) =>
        new(Guid.Parse(proto.NoteTypeId), proto.Name);

    public static NoteTypeNotification ToNotification(this ChangeNoteTypeNameCommandProto proto) =>
        new()
        {
            NoteTypeNameChanged = new NoteTypeNameChangedNotification()
            {
                NoteTypeId = proto.NoteTypeId,
                Name = proto.Name,
            }
        };

    public static ChangeNoteTypeColorCommand ToCommand(this ChangeNoteTypeColorCommandProto proto) =>
        new(Guid.Parse(proto.NoteTypeId), proto.Color);

    public static NoteTypeNotification ToNotification(this ChangeNoteTypeColorCommandProto proto) =>
        new()
        {
            NoteTypeColorChanged = new NoteTypeColorChangedNotification()
            {
                NoteTypeId = proto.NoteTypeId,
                Color = proto.Color
            }
        };

    public static GetAllNoteTypesResponseProto ToProtoList(this List<Domain.Entities.NoteType> noteTypes)
    {
        var noteTypeProtos = new GetAllNoteTypesResponseProto();

        foreach (var noteType in noteTypes)
        {
            noteTypeProtos.NoteTypes.Add(noteType.ToProto());
        }

        return noteTypeProtos;
    }

    public static NoteTypeProto ToProto(this Domain.Entities.NoteType noteType) =>
        new()
        {
            NoteTypeId = noteType.NoteTypeId.ToString(),
            Name = noteType.Name,
            Color = noteType.Color,
        };
}