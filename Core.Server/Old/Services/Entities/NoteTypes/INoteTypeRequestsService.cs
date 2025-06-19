namespace Service.Server.Old.Services.Entities.NoteTypes;

public interface INoteTypeRequestsService
{
    Task<List<NoteTypeDto>> GetAll();
    Task<NoteTypeDto?> GetById(Guid id);
}