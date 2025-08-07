using Core.Persistence.DbContext;
using Domain.Events.Sprints;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EventStores;

public class SprintEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory) : IEventStore<SprintEvent>
{
    public async Task StoreEventAsync(SprintEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.SprintEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<SprintEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.SprintEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<SprintEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.SprintEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}