using Domain.Entities;

namespace Service.Server.Services.Entities.Tags;

public interface ITagRequestsService
{
    Task<Tag> GetTagById(Guid tagId);
    Task<List<Tag>> GetTagsByTagIds(List<Guid> tagIds);
    Task<List<Tag>> GetAll();
}