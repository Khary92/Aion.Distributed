using Core.Server.Services.Entities.Notes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.Requests.Notes;

namespace Core.Server.Communication.Endpoints.Note;

[Authorize]
public class NoteRequestReceiver(INoteRequestsService noteRequestsService)
    : NotesRequestService.NotesRequestServiceBase
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
        var notesByTimeSlotId = await noteRequestsService.GetNotesByTimeSlotId(Guid.Parse(request.TimeSlotId));
        return notesByTimeSlotId.ToProtoList();
    }
}