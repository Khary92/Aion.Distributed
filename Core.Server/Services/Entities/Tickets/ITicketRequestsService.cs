using Domain.Entities;

namespace Service.Server.Services.Entities.Tickets;

public interface ITicketRequestsService
{
    Task<Ticket?> GetTicketById(Guid ticketId);
    Task<List<Ticket>> GetAll();
    Task<List<Ticket>> GetTicketsBySprintId(Guid domainSprintSprintId);
    Task<List<Ticket>> GetTicketsForCurrentSprint();
}