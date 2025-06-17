using Application.Contract.CQRS.Requests.Notes;
using Application.Contract.DTO;
using Application.Services.Entities.Notes;
using MediatR;

namespace Application.Handler.Requests.Notes;

public class GetNotesByTimeSlotIdRequestHandler(INoteRequestsService noteRequestsService)
    : IRequestHandler<GetNotesByTimeSlotIdRequest, List<NoteDto>>
{
    public async Task<List<NoteDto>> Handle(GetNotesByTimeSlotIdRequest request, CancellationToken cancellationToken)
    {
        return await noteRequestsService.GetNotesByTimeSlotId(request.TimeSlotId);
    }
}