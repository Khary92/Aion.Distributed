using Domain.Entities;

namespace Core.Server.Services.Entities.NoteTypes;

public interface INoteTypeRequestsService
{
    Task<List<NoteType>> GetAll();
    Task<NoteType?> GetById(Guid id);
}