using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests.Tickets;

public interface ITicketRequestSender
{
    Task<List<TicketDto>> Send(GetAllTicketsRequestProto request);
    Task<List<TicketDto>> Send(GetTicketsForCurrentSprintRequestProto request);
    Task<List<TicketDto>> Send(GetTicketsWithShowAllSwitchRequestProto request);
    Task<TicketDto> Send(GetTicketByIdRequestProto request);
}