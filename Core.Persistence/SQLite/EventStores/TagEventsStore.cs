using Domain.Events.Tags;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class TagEventsStore(AppDbContext appDbContext) : IEventStore<TagEvent>
{
    public async Task StoreEventAsync(TagEvent @event)
    {
        await appDbContext.TagEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TagEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.TagEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TagEvent>> GetAllEventsAsync()
    {
        return await appDbContext.TagEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}