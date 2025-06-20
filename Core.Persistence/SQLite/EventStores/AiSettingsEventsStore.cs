using Domain.Events.AiSettings;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class AiSettingsEventsStore(AppDbContext appDbContext) : IEventStore<AiSettingsEvent>
{
    public async Task StoreEventAsync(AiSettingsEvent @event)
    {
        await appDbContext.AiSettingsEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<AiSettingsEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.AiSettingsEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AiSettingsEvent>> GetAllEventsAsync()
    {
        return await appDbContext.AiSettingsEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}