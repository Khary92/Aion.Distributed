using Core.Server.Communication.CQRS.Commands.Entities.Note;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Notes;
using Proto.DTO.Note;
using Proto.Notifications.Note;
using Proto.Requests.Notes;

namespace Core.Server.Communication.Services.Note;

public static class NoteProtoExtensions
{
    public static CreateNoteCommand ToCommand(this CreateNoteCommandProto proto)
    {
        return new CreateNoteCommand(Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId),
            Guid.Parse(proto.TimeSlotId),
            proto.TimeStamp.ToDateTimeOffset());
    }

    public static NoteNotification ToNotification(this CreateNoteCommand proto)
    {
        return new NoteNotification
        {
            NoteCreated = new NoteCreatedNotification
            {
                NoteId = proto.NoteId.ToString(),
                Text = proto.Text,
                NoteTypeId = proto.NoteTypeId.ToString(),
                TimeSlotId = proto.TimeSlotId.ToString(),
                TimeStamp = proto.TimeStamp.ToTimestamp()
            }
        };
    }

    public static UpdateNoteCommand ToCommand(this UpdateNoteCommandProto proto)
    {
        return new UpdateNoteCommand(Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId),
            Guid.Parse(proto.TimeSlotId));
    }

    public static NoteNotification ToNotification(this UpdateNoteCommand proto)
    {
        return new NoteNotification
        {
            NoteUpdated = new NoteUpdatedNotification
            {
                NoteId = proto.NoteId.ToString(),
                Text = proto.Text,
                NoteTypeId = proto.NoteTypeId.ToString(),
                TimeSlotId = proto.TimeSlotId.ToString()
            }
        };
    }

    public static GetNotesResponseProto ToProtoList(this List<Domain.Entities.Note> notes)
    {
        var noteProtos = new GetNotesResponseProto();

        foreach (var note in notes) noteProtos.Notes.Add(note.ToProto());

        return noteProtos;
    }

    private static NoteProto ToProto(this Domain.Entities.Note note)
    {
        return new NoteProto
        {
            NoteId = note.NoteId.ToString(),
            NoteTypeId = note.NoteTypeId.ToString(),
            Text = note.Text,
            TimeSlotId = note.TimeSlotId.ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(note.TimeStamp)
        };
    }
}