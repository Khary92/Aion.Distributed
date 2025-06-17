using Application.Contract.DTO;
using Application.Mapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services.Entities.Tickets;

public class TicketRequestService(
    ITicketEventsStore ticketEventStore,
    IDtoMapper<TicketDto, Ticket> ticketMapper) : ITicketRequestsService
{
    public async Task<TicketDto?> GetTicketAsync(Guid ticketId)
    {
        var ticketEvents = await ticketEventStore.GetEventsForAggregateAsync(ticketId);

        var domainTicket = Ticket.Rehydrate(ticketEvents);
        return ticketMapper.ToDto(domainTicket);
    }

    public async Task<List<TicketDto>> GetAll()
    {
        var allEvents = await ticketEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => Ticket.Rehydrate(group.ToList()))
            .Select(ticketMapper.ToDto)
            .ToList();
    }

    public async Task<List<TicketDto>> GetTicketsBySprintId(Guid sprintId)
    {
        var allEvents = await ticketEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .ToList();

        return groupedEvents
            .Select(group => Ticket.Rehydrate(group.ToList()))
            .Select(ticketMapper.ToDto)
            .Where(t => t.SprintIds.Contains(sprintId))
            .ToList();
    }
}