using Core.Persistence.DbContext;
using Domain.Events.TimeSlots;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EventStores;

public class TimeSlotEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory) : IEventStore<TimeSlotEvent>
{
    public async Task StoreEventAsync(TimeSlotEvent @event, Guid traceId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.TimeSlotEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TimeSlotEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.TimeSlotEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TimeSlotEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.TimeSlotEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}