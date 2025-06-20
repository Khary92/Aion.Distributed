using Domain.Events.StatisticsData;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class StatisticsDataEventsStore(AppDbContext appDbContext) : IEventStore<StatisticsDataEvent>
{
    public async Task StoreEventAsync(StatisticsDataEvent @event)
    {
        await appDbContext.StatisticsDataEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<StatisticsDataEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.StatisticsDataEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<StatisticsDataEvent>> GetAllEventsAsync()
    {
        return await appDbContext.StatisticsDataEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}