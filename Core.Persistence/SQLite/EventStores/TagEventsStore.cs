using Core.Persistence.SQLite.DbContext;
using Domain.Events.Tags;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.SQLite.EventStores;

public class TagEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory) : IEventStore<TagEvent>
{
    public async Task StoreEventAsync(TagEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();
        
        await appDbContext.TagEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TagEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();
        
        return await appDbContext.TagEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TagEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();
        
        return await appDbContext.TagEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}