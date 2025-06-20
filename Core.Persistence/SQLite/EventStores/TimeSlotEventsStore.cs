using Domain.Events.TimeSlots;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class TimeSlotEventsStore(AppDbContext appDbContext) : IEventStore<TimeSlotEvent>
{
    public async Task StoreEventAsync(TimeSlotEvent @event)
    {
        await appDbContext.TimeSlotEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TimeSlotEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.TimeSlotEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TimeSlotEvent>> GetAllEventsAsync()
    {
        return await appDbContext.TimeSlotEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}