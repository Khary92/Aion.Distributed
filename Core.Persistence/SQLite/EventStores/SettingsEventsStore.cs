using Domain.Events.Settings;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class SettingsEventsStore(AppDbContext appDbContext) : IEventStore<SettingsEvent>
{
    public async Task StoreEventAsync(SettingsEvent @event)
    {
        await appDbContext.SettingsEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<SettingsEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.SettingsEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<SettingsEvent>> GetAllEventsAsync()
    {
        return await appDbContext.SettingsEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}