using Domain.Events.NoteTypes;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class NoteTypeEventsStore(AppDbContext appDbContext) : IEventStore<NoteTypeEvent>
{
    public async Task StoreEventAsync(NoteTypeEvent @event)
    {
        await appDbContext.NoteTypeEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<NoteTypeEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.NoteTypeEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<NoteTypeEvent>> GetAllEventsAsync()
    {
        return await appDbContext.NoteTypeEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}