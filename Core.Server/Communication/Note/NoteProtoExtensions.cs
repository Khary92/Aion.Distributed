using Google.Protobuf.WellKnownTypes;
using Proto.Command.Notes;
using Proto.DTO.Note;
using Proto.Notifications.Note;
using Proto.Requests.Notes;
using Service.Server.CQRS.Commands.Entities.Note;

namespace Service.Server.Communication.Note;

public static class NoteProtoExtensions
{
    public static CreateNoteCommand ToCommand(this CreateNoteCommandProto proto) =>
        new(Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId), Guid.Parse(proto.TimeSlotId),
            proto.TimeStamp.ToDateTimeOffset());

    public static NoteNotification ToNotification(this CreateNoteCommandProto proto) =>
        new()
        {
            NoteCreated = new NoteCreatedNotification
            {
                NoteId = proto.NoteId,
                Text = proto.Text,
                NoteTypeId = proto.NoteTypeId,
                TimeSlotId = proto.TimeSlotId,
                TimeStamp = proto.TimeStamp
            }
        };

    public static UpdateNoteCommand ToCommand(this UpdateNoteCommandProto proto) =>
        new(Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId), Guid.Parse(proto.TimeSlotId));

    public static NoteNotification ToNotification(this UpdateNoteCommandProto proto) =>
        new()
        {
            NoteUpdated = new NoteUpdatedNotification
            {
                NoteId = proto.NoteId,
                Text = proto.Text,
                NoteTypeId = proto.NoteTypeId,
                TimeSlotId = proto.TimeSlotId
            }
        };
    
    public static GetNotesResponseProto ToProtoList(this List<Domain.Entities.Note> notes)
    {
        var noteProtos = new GetNotesResponseProto();

        foreach (var note in notes)
        {
            noteProtos.Notes.Add(note.ToProto());
        }
        
        return noteProtos;
    }
    
    private static NoteProto ToProto(this Domain.Entities.Note note) =>
        new()
        {
            NoteId = note.NoteId.ToString(),
            NoteTypeId = note.NoteTypeId.ToString(),
            Text = note.Text,
            TimeSlotId = note.TimeSlotId.ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(note.TimeStamp)
        };
}