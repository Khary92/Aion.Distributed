using Domain.Entities;
using Domain.Interfaces;
using Service.Server.Communication.Mapper;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Old.Services.Entities.Tickets;

public class TicketRequestService(
    ITicketEventsStore ticketEventStore, ISprintRequestsService sprintRequestsService) : ITicketRequestsService
{
    public async Task<Ticket?> GetTicketById(Guid ticketId)
    {
        var ticketEvents = await ticketEventStore.GetEventsForAggregateAsync(ticketId);

        return Ticket.Rehydrate(ticketEvents);
    }

    public async Task<List<Ticket>> GetAll()
    {
        var allEvents = await ticketEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => Ticket.Rehydrate(group.ToList()))
            .ToList();
    }

    public async Task<List<Ticket>> GetTicketsBySprintId(Guid sprintId)
    {
        var allEvents = await ticketEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .ToList();

        return groupedEvents
            .Select(group => Ticket.Rehydrate(group.ToList()))
            .Where(t => t.SprintIds.Contains(sprintId))
            .ToList();
    }

    public async Task<List<Ticket>> GetTicketsForCurrentSprint()
    {
        var activeSprint = await sprintRequestsService.GetActiveSprint();

        if (activeSprint == null) return [];
        
        return await GetTicketsBySprintId(activeSprint.SprintId);
    }
}