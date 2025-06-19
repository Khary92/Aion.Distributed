namespace Service.Server.Old.Services.Entities.Tags;

public interface ITagRequestsService
{
    Task<TagDto> GetTagById(Guid tagId);
    Task<List<TagDto>> GetTagsByTagIds(List<Guid> tagIds);
    Task<List<TagDto>> GetAll();
}