using Domain.Entities;
using Domain.Events.Sprints;
using Domain.Interfaces;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.Sprints;

public class SprintRequestService(
    IEventStore<SprintEvent> sprintEventsStore,
    IDtoMapper<SprintDto, Sprint> sprintMapper) : ISprintRequestsService
{
    public async Task<List<SprintDto>> GetAll()
    {
        var tickets = (await sprintEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Sprint.Rehydrate).ToList();

        return tickets.Select(sprintMapper.ToDto).ToList();
    }

    public async Task<SprintDto?> GetById(Guid id)
    {
        var sprintEvents = await sprintEventsStore
            .GetEventsForAggregateAsync(id);

        var domainSprint = Sprint.Rehydrate(sprintEvents);
        return sprintMapper.ToDto(domainSprint);
    }
}