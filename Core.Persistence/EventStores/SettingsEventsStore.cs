using Core.Persistence.DbContext;
using Domain.Events.Settings;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EventStores;

public class SettingsEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory) : IEventStore<SettingsEvent>
{
    public async Task StoreEventAsync(SettingsEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.SettingsEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<SettingsEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.SettingsEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<SettingsEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.SettingsEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}