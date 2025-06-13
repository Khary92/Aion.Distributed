using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Proto.Requests.Notes;

public class NoteRequestServiceImpl : NotesRequestService.NotesRequestServiceBase
{
    public override Task<GetNotesResponseProto> GetNotesByTicketId(GetNotesByTicketIdRequestProto request,
        ServerCallContext context)
    {
        var notes = new GetNotesResponseProto();
        notes.Notes.Add(new NoteProto
        {
            NoteId = "note-123",
            Text = "Beispielnotiz für Ticket " + request.TicketId,
            NoteTypeId = "type-1",
            TimeSlotId = "timeslot-1",
            TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        return Task.FromResult(notes);
    }

    public override Task<GetNotesResponseProto> GetNotesByTimeSlotId(GetNotesByTimeSlotIdRequestProto request,
        ServerCallContext context)
    {
        var notes = new GetNotesResponseProto();
        notes.Notes.Add(new NoteProto
        {
            NoteId = "note-456",
            Text = "Beispielnotiz für TimeSlot " + request.TimeSlotId,
            NoteTypeId = "type-2",
            TimeSlotId = request.TimeSlotId,
            TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        return Task.FromResult(notes);
    }
}