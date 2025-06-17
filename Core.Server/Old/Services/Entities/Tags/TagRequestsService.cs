using Application.Contract.DTO;
using Application.Mapper;
using Domain.Entities;
using Domain.Events.Tags;
using Domain.Interfaces;

namespace Application.Services.Entities.Tags;

public class TagRequestsService(IEventStore<TagEvent> tagEventStore, IDtoMapper<TagDto, Tag> tagMapper)
    : ITagRequestsService
{
    public async Task<TagDto> GetTagById(Guid tagId)
    {
        var sprintEvents = await tagEventStore
            .GetEventsForAggregateAsync(tagId);

        var domainSprint = Tag.Rehydrate(sprintEvents);
        return tagMapper.ToDto(domainSprint);
    }

    public async Task<List<TagDto>> GetTagsByTagIds(List<Guid> tagIds)
    {
        var tagDtos = await GetAll();
        return tagDtos.Where(tag => tagIds.Contains(tag.TagId)).ToList();
    }

    public async Task<List<TagDto>> GetAll()
    {
        var allEvents = await tagEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => Tag.Rehydrate(group.ToList()))
            .Select(tagMapper.ToDto)
            .ToList();
    }
}