using Domain.Entities;

namespace Service.Server.Old.Services.Entities.Sprints;

public interface ISprintRequestsService
{
    Task<List<Sprint>> GetAll();
    Task<Sprint> GetById(Guid id);
}