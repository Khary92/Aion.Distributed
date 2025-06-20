using Domain.Interfaces;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Services.UseCase.Replays;

public class ReplayRequestsService(ITicketRequestsService ticketRequestsService, ITicketEventsStore ticketEventsStore)
    : IReplayRequestsService
{
    public async Task<TicketReplayDecorator> GetTicketReplayById(Guid ticketId)
    {
        var ticket = await ticketRequestsService.GetTicketById(ticketId);

        if (ticket == null) throw new Exception("Ticket not found");

        return new TicketReplayDecorator(ticket.Clone(), ticketEventsStore);
    }
}