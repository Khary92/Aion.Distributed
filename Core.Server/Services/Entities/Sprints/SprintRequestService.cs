using Domain.Entities;
using Domain.Events.Sprints;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Sprints;

public class SprintRequestService(IEventStore<SprintEvent> sprintEventsStore) : ISprintRequestsService
{
    public async Task<List<Sprint>> GetAll()
    {
        var tickets = (await sprintEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Sprint.Rehydrate).ToList();

        return tickets;
    }

    public async Task<Sprint?> GetById(Guid id)
    {
        var sprintEvents = await sprintEventsStore
            .GetEventsForAggregateAsync(id);

        return Sprint.Rehydrate(sprintEvents);
    }

    public async Task<Sprint?> GetActiveSprint()
    {
        return (await GetAll()).FirstOrDefault(s => s.IsActive);
    }
}