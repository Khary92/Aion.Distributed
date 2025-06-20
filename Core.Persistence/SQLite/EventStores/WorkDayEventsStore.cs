using Domain.Events.WorkDays;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.SQLite.EventStores;

public class WorkDayEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory) : IEventStore<WorkDayEvent>
{
    public async Task StoreEventAsync(WorkDayEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();
        
        await appDbContext.WorkDayEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<WorkDayEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();
        
        return await appDbContext.WorkDayEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<WorkDayEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();
        
        return await appDbContext.WorkDayEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}