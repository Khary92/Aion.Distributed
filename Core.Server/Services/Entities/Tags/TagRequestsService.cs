using Domain.Entities;
using Domain.Events.Tags;
using Domain.Interfaces;

namespace Service.Server.Services.Entities.Tags;

public class TagRequestsService(IEventStore<TagEvent> tagEventStore)
    : ITagRequestsService
{
    public async Task<Tag> GetTagById(Guid tagId)
    {
        var sprintEvents = await tagEventStore
            .GetEventsForAggregateAsync(tagId);

        var domainSprint = Tag.Rehydrate(sprintEvents);
        return domainSprint;
    }

    public async Task<List<Tag>> GetTagsByTagIds(List<Guid> tagIds)
    {
        var tagDtos = await GetAll();
        return tagDtos.Where(tag => tagIds.Contains(tag.TagId)).ToList();
    }

    public async Task<List<Tag>> GetAll()
    {
        var allEvents = await tagEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => Tag.Rehydrate(group.ToList()))
            .ToList();
    }
}