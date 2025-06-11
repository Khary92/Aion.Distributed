using System.Threading.Tasks;
using Proto.Requests.Tickets;

namespace Client.Avalonia.Communication.Requests;

public interface ITicketRequestSender
{
    Task<TicketListProto> GetAllTickets();
    Task<TicketListProto> GetTicketsForCurrentSprint();
    Task<TicketListProto> GetTicketsWithShowAllSwitch(bool isShowAll);
}