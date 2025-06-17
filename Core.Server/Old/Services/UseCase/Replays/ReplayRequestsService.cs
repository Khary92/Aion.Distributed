using Application.Contract.DTO;
using Application.Decorators;
using Application.Services.Entities.Tickets;
using Domain.Interfaces;

namespace Application.Services.UseCase.Replays;

public class ReplayRequestsService(ITicketRequestsService ticketRequestsService, ITicketEventsStore ticketEventsStore)
    : IReplayRequestsService
{
    public async Task<TicketReplayDecorator> GetTicketReplayById(Guid ticketId)
    {
        var ticket = await ticketRequestsService.GetTicketAsync(ticketId);

        if (ticket == null) throw new Exception("Ticket not found");

        return new TicketReplayDecorator(ticket.Clone(), ticketEventsStore);
    }
}