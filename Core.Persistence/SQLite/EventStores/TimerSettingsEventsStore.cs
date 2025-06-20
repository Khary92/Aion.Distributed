using Domain.Events.TimerSettings;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class TimerSettingsEventsStore(AppDbContext appDbContext) : IEventStore<TimerSettingsEvent>
{
    public async Task StoreEventAsync(TimerSettingsEvent @event)
    {
        await appDbContext.TimerSettingsEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TimerSettingsEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.TimerSettingsEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TimerSettingsEvent>> GetAllEventsAsync()
    {
        return await appDbContext.TimerSettingsEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}