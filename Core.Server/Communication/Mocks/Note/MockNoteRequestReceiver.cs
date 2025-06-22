using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Proto.DTO.Note;
using Proto.Requests.Notes;

namespace Core.Server.Communication.Mocks.Note;

public class MockNoteRequestReceiver : NotesRequestService.NotesRequestServiceBase
{
    public override Task<GetNotesResponseProto> GetNotesByTicketId(GetNotesByTicketIdRequestProto request,
        ServerCallContext context)
    {
        var notes = new GetNotesResponseProto();
        notes.Notes.Add(new NoteProto
        {
            NoteId = Guid.NewGuid().ToString(),
            Text = "Beispielnotiz für Ticket " + request.TicketId,
            NoteTypeId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
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
            NoteId = Guid.NewGuid().ToString(),
            Text = "Beispielnotiz für TimeSlot " + request.TimeSlotId,
            NoteTypeId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        return Task.FromResult(notes);
    }
}