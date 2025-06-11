using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.Tickets;

namespace Client.Avalonia.Communication.Requests.Tickets;

public interface ITicketRequestSender
{
    Task<List<TicketDto>> GetAllTickets();
    Task<List<TicketDto>> GetTicketsForCurrentSprint();
    Task<List<TicketDto>> GetTicketsWithShowAllSwitch(bool isShowAll);
}