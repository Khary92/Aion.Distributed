using Domain.Entities;

namespace Core.Server.Services.Entities.Notes;

public interface INoteRequestsService
{
    Task<List<Note>> GetNotesByTimeSlotId(Guid timeSlotId);
    Task<List<Note>> GetNotesByTicketId(Guid ticketId);
}