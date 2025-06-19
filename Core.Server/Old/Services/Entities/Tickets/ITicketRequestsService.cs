namespace Service.Server.Old.Services.Entities.Tickets;

public interface ITicketRequestsService
{
    Task<TicketDto?> GetTicketAsync(Guid ticketId);
    Task<List<TicketDto>> GetAll();
    Task<List<TicketDto>> GetTicketsBySprintId(Guid domainSprintSprintId);
}