using Domain.Entities;

namespace Service.Server.Services.Entities.NoteTypes;

public interface INoteTypeRequestsService
{
    Task<List<NoteType>> GetAll();
    Task<NoteType?> GetById(Guid id);
}