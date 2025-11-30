using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests.Ticket;

public interface ITicketRequestSender
{
    Task<List<TicketClientModel>> Send(GetAllTicketsRequestProto request);
    Task<List<TicketClientModel>> Send(GetTicketsForCurrentSprintRequestProto request);
    Task<List<TicketClientModel>> Send(GetTicketsWithShowAllSwitchRequestProto request);
    Task<TicketClientModel> Send(GetTicketByIdRequestProto request);
}