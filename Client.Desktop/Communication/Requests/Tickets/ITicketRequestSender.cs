using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.Tickets;

public interface ITicketRequestSender
{
    Task<List<TicketDto>> GetAllTickets();
    Task<List<TicketDto>> GetTicketsForCurrentSprint();
    Task<List<TicketDto>> GetTicketsWithShowAllSwitch(bool isShowAll);
}