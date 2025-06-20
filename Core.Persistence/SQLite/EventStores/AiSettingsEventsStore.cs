using Core.Persistence.SQLite.DbContext;
using Domain.Events.AiSettings;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.SQLite.EventStores;

public class AiSettingsEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory) : IEventStore<AiSettingsEvent>
{
    public async Task StoreEventAsync(AiSettingsEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.AiSettingsEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<AiSettingsEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.AiSettingsEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<AiSettingsEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.AiSettingsEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}