using Core.Persistence.SQLite.DbContext;
using Domain.Events.NoteTypes;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.SQLite.EventStores;

public class NoteTypeEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory) : IEventStore<NoteTypeEvent>
{
    public async Task StoreEventAsync(NoteTypeEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.NoteTypeEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<NoteTypeEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.NoteTypeEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<NoteTypeEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.NoteTypeEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }
}