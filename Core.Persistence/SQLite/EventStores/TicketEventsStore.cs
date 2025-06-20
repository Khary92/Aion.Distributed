using System.Text.Json;
using Domain.Events.Tickets;
using Domain.Interfaces;
using Infrastructure.SQLite.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLite.EventStores;

public class TicketEventsStore(AppDbContext appDbContext) : ITicketEventsStore
{
    public async Task StoreEventAsync(TicketEvent @event)
    {
        await appDbContext.TicketEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TicketEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        return await appDbContext.TicketEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TicketEvent>> GetAllEventsAsync()
    {
        return await appDbContext.TicketEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TicketDocumentationChangedEvent>> GetTicketDocumentationEventsByTicketId(Guid ticketId)
    {
        var rawEvents = await appDbContext.TicketEvents
            .Where(e => e.EntityId == ticketId && e.EventType == nameof(TicketDocumentationChangedEvent))
            .OrderBy(e => e.TimeStamp)
            .Select(e => e.EventPayload)
            .ToListAsync();

        return rawEvents
            .Select(payload => JsonSerializer.Deserialize<TicketDocumentationChangedEvent>(payload))
            .Where(e => e != null)
            .ToList()!;
    }
}