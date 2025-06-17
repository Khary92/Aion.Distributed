using Application.Contract.DTO;

namespace Application.Services.Entities.Sprints;

public interface ISprintRequestsService
{
    Task<List<SprintDto>> GetAll();
    Task<SprintDto?> GetById(Guid id);
}