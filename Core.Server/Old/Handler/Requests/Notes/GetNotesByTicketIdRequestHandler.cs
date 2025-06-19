using Service.Server.CQRS.Requests.Notes;
using Service.Server.Old.Services.Entities.Notes;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Requests.Notes;

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