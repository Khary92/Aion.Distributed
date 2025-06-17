using Application.Contract.CQRS.Requests.Notes;
using Application.Contract.DTO;
using Application.Services.Entities.Notes;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Requests.Notes;

public class GetNotesByTicketIdRequestHandler(
    ITimeSlotRequestsService timeSlotRequestsService,
    INoteRequestsService noteRequestsService)
    : IRequestHandler<GetNotesByTicketIdRequest, List<NoteDto>>

{
    public async Task<List<NoteDto>> Handle(GetNotesByTicketIdRequest request, CancellationToken cancellationToken)
    {
        var timeSlots = await timeSlotRequestsService.GetAll();

        var list = timeSlots.Where(ts => ts.SelectedTicketId == request.TicketId);

        var noteDtos = new List<NoteDto>();
        foreach (var timeSlot in list)
            noteDtos.AddRange(await noteRequestsService.GetNotesByTimeSlotId(timeSlot.TimeSlotId));

        return noteDtos;
    }
}