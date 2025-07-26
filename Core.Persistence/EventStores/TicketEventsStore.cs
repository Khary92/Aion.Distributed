using System.Text.Json;
using Core.Persistence.DbContext;
using Core.Server.Tracing.Tracing.Tracers;
using Domain.Events.Tickets;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EventStores;

public class TicketEventsStore(IDbContextFactory<AppDbContext> appDbContextFactory, ITraceCollector tracer)
    : ITicketEventsStore
{
    public async Task StoreEventAsync(TicketEvent @event)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        await appDbContext.TicketEvents.AddAsync(@event);
        await appDbContext.SaveChangesAsync();

        await tracer.Ticket.Create.EventPersisted(GetType(), @event.EntityId, @event);
    }

    public async Task<List<TicketEvent>> GetEventsForAggregateAsync(Guid entityId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.TicketEvents
            .Where(e => e.EntityId == entityId)
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TicketEvent>> GetAllEventsAsync()
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

        return await appDbContext.TicketEvents
            .OrderBy(e => e.TimeStamp)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<TicketDocumentationChangedEvent>> GetTicketDocumentationEventsByTicketId(Guid ticketId)
    {
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();

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