using Core.Persistence.DbContext;
using Domain.Events.StatisticsData;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EventStores;

public class StatisticsDataEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory)
    : IEventStore<StatisticsDataEvent>
{
    public async Task StoreEventAsync(StatisticsDataEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.StatisticsDataEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<StatisticsDataEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.StatisticsDataEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<StatisticsDataEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.StatisticsDataEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}