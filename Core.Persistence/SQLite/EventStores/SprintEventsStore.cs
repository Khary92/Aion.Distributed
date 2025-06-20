using Domain.Events.Sprints;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class SprintEventsStore(AppDbContext appDbContext) : IEventStore<SprintEvent>
{
    public async Task StoreEventAsync(SprintEvent @event)
    {
        await appDbContext.SprintEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<SprintEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.SprintEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<SprintEvent>> GetAllEventsAsync()
    {
        return await appDbContext.SprintEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}