using Core.Server.Communication.Records.Commands.Entities.NoteType;
using Proto.Command.NoteTypes;
using Proto.DTO.NoteType;
using Proto.DTO.TraceData;
using Proto.Notifications.NoteType;
using Proto.Requests.NoteTypes;

namespace Core.Server.Communication.Endpoints.NoteType;

public static class NoteTypeProtoExtensions
{
    public static CreateNoteTypeCommand ToCommand(this CreateNoteTypeCommandProto proto)
    {
        return new CreateNoteTypeCommand(
            Guid.Parse(proto.NoteTypeId), proto.Name, proto.Color, Guid.Parse(proto.TraceData.TraceId));
    }

    public static NoteTypeNotification ToNotification(this CreateNoteTypeCommand command)
    {
        return new NoteTypeNotification
        {
            NoteTypeCreated = new NoteTypeCreatedNotification
            {
                NoteTypeId = command.NoteTypeId.ToString(),
                Name = command.Name,
                Color = command.Color,
                TraceData = new TraceDataProto
                {
                    TraceId = command.TraceId.ToString()
                }
            }
        };
    }

    public static ChangeNoteTypeNameCommand ToCommand(this ChangeNoteTypeNameCommandProto proto)
    {
        return new ChangeNoteTypeNameCommand(
            Guid.Parse(proto.NoteTypeId), proto.Name,
            Guid.Parse(proto.TraceData.TraceId));
    }

    public static NoteTypeNotification ToNotification(this ChangeNoteTypeNameCommand proto)
    {
        return new NoteTypeNotification
        {
            NoteTypeNameChanged = new NoteTypeNameChangedNotification
            {
                NoteTypeId = proto.NoteTypeId.ToString(),
                Name = proto.Name,
                TraceData = new TraceDataProto
                {
                    TraceId = proto.TraceId.ToString()
                }
            }
        };
    }

    public static ChangeNoteTypeColorCommand ToCommand(this ChangeNoteTypeColorCommandProto proto)
    {
        return new ChangeNoteTypeColorCommand(
            Guid.Parse(proto.NoteTypeId), proto.Color, Guid.Parse(proto.TraceData.TraceId));
    }

    public static NoteTypeNotification ToNotification(this ChangeNoteTypeColorCommand proto)
    {
        return new NoteTypeNotification
        {
            NoteTypeColorChanged = new NoteTypeColorChangedNotification
            {
                NoteTypeId = proto.NoteTypeId.ToString(),
                Color = proto.Color,
                TraceData = new TraceDataProto
                {
                    TraceId = proto.TraceId.ToString()
                }
            }
        };
    }

    public static GetAllNoteTypesResponseProto ToProtoList(this List<Domain.Entities.NoteType> noteTypes)
    {
        var noteTypeProtos = new GetAllNoteTypesResponseProto();

        foreach (var noteType in noteTypes) noteTypeProtos.NoteTypes.Add(noteType.ToProto());

        return noteTypeProtos;
    }

    public static NoteTypeProto ToProto(this Domain.Entities.NoteType noteType)
    {
        return new NoteTypeProto
        {
            NoteTypeId = noteType.NoteTypeId.ToString(),
            Name = noteType.Name,
            Color = noteType.Color
        };
    }
}