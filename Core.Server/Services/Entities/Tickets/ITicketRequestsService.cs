using Domain.Entities;

namespace Core.Server.Services.Entities.Tickets;

public interface ITicketRequestsService
{
    Task<Ticket?> GetTicketById(Guid ticketId);
    Task<List<Ticket>> GetAll();
    Task<List<Ticket>> GetTicketsBySprintId(Guid domainSprintSprintId);
    Task<List<Ticket>> GetTicketsForCurrentSprint();
    Task<List<string>> GetDocumentationByTicketId(Guid ticketId);
}