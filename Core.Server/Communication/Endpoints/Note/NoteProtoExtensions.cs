using Core.Server.Communication.Records.Commands.Entities.Note;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Notes;
using Proto.DTO.Note;
using Proto.DTO.TraceData;
using Proto.Notifications.Note;
using Proto.Requests.Notes;

namespace Core.Server.Communication.Endpoints.Note;

public static class NoteProtoExtensions
{
    public static CreateNoteCommand ToCommand(this CreateNoteCommandProto proto) => new(
        Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId),
        Guid.Parse(proto.TicketId), Guid.Parse(proto.TimeSlotId),
        proto.TimeStamp.ToDateTimeOffset(), Guid.Parse(proto.TraceData.TraceId));

    public static NoteNotification ToNotification(this CreateNoteCommand proto) => new()
    {
        NoteCreated = new NoteCreatedNotification
        {
            NoteId = proto.NoteId.ToString(),
            Text = proto.Text,
            NoteTypeId = proto.NoteTypeId.ToString(),
            TimeSlotId = proto.TimeSlotId.ToString(),
            TimeStamp = proto.TimeStamp.ToTimestamp(),
            TraceData = new TraceDataProto
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static UpdateNoteCommand ToCommand(this UpdateNoteCommandProto proto) => new(Guid.Parse(proto.NoteId),
        proto.Text, Guid.Parse(proto.NoteTypeId), Guid.Parse(proto.TimeSlotId),
        Guid.Parse(proto.TraceData.TraceId));

    public static NoteNotification ToNotification(this UpdateNoteCommand proto) => new()
    {
        NoteUpdated = new NoteUpdatedNotification
        {
            NoteId = proto.NoteId.ToString(),
            Text = proto.Text,
            NoteTypeId = proto.NoteTypeId.ToString(),
            TimeSlotId = proto.TimeSlotId.ToString(),
            TraceData = new TraceDataProto
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static GetNotesResponseProto ToProtoList(this List<Domain.Entities.Note> notes)
    {
        var noteProtos = new GetNotesResponseProto();

        foreach (var note in notes) noteProtos.Notes.Add(note.ToProto());

        return noteProtos;
    }

    private static NoteProto ToProto(this Domain.Entities.Note note) => new()
    {
        NoteId = note.NoteId.ToString(),
        NoteTypeId = note.NoteTypeId.ToString(),
        Text = note.Text,
        TimeSlotId = note.TicketId.ToString(),
        TimeStamp = Timestamp.FromDateTimeOffset(note.TimeStamp)
    };
}