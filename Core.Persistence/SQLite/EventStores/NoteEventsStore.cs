using Domain.Events.Note;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class NoteEventsStore(AppDbContext appDbContext) : IEventStore<NoteEvent>
{
    public async Task StoreEventAsync(NoteEvent @event)
    {
        await appDbContext.NoteEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<NoteEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.NoteEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<NoteEvent>> GetAllEventsAsync()
    {
        return await appDbContext.NoteEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}