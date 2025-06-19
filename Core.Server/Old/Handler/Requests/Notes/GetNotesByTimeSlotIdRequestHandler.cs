using Service.Server.CQRS.Requests.Notes;
using Service.Server.Old.Services.Entities.Notes;

namespace Service.Server.Old.Handler.Requests.Notes;

public class GetNotesByTimeSlotIdRequestHandler(INoteRequestsService noteRequestsService)
    : IRequestHandler<GetNotesByTimeSlotIdRequest, List<NoteDto>>
{
    public async Task<List<NoteDto>> Handle(GetNotesByTimeSlotIdRequest request, CancellationToken cancellationToken)
    {
        return await noteRequestsService.GetNotesByTimeSlotId(request.TimeSlotId);
    }
}