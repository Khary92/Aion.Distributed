using Core.Persistence.DbContext;
using Domain.Events.TimerSettings;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EventStores;

public class TimerSettingsEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory)
    : IEventStore<TimerSettingsEvent>
{
    public async Task StoreEventAsync(TimerSettingsEvent @event, Guid traceId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.TimerSettingsEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TimerSettingsEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.TimerSettingsEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TimerSettingsEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.TimerSettingsEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}