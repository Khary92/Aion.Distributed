using Domain.Entities;

namespace Service.Server.Old.Services.Entities.Notes;

public interface INoteRequestsService
{
    Task<List<Note>> GetNotesByTimeSlotId(Guid timeSlotId);
    Task<List<Note>> GetNotesByTicketId(Guid ticketId);
}