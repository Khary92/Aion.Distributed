namespace Service.Server.Old.Services.Entities.Notes;

public interface INoteRequestsService
{
    Task<List<NoteDto>> GetNotesByTimeSlotId(Guid timeSlotId);
}