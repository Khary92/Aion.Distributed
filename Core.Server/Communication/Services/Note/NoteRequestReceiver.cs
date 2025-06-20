using Grpc.Core;
using Proto.Requests.Notes;
using Service.Server.Services.Entities.Notes;

namespace Service.Server.Communication.Services.Note;

public class NoteRequestReceiver(INoteRequestsService noteRequestsService) : NotesRequestService.NotesRequestServiceBase
{
    public override async Task<GetNotesResponseProto> GetNotesByTicketId(GetNotesByTicketIdRequestProto request,
        ServerCallContext context)
    {
        var notesByTicketId = await noteRequestsService.GetNotesByTicketId(Guid.Parse(request.TicketId));
        return notesByTicketId.ToProtoList();
    }

    public override async Task<GetNotesResponseProto> GetNotesByTimeSlotId(GetNotesByTimeSlotIdRequestProto request,
        ServerCallContext context)
    {
        var notesByTimeSlotId = await noteRequestsService.GetNotesByTicketId(Guid.Parse(request.TimeSlotId));
        return notesByTimeSlotId.ToProtoList();
    }
}