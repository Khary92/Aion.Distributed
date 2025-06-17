using Application.Contract.DTO;

namespace Application.Services.Entities.Notes;

public interface INoteRequestsService
{
    Task<List<NoteDto>> GetNotesByTimeSlotId(Guid timeSlotId);
}