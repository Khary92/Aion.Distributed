using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.Requests.Sprints;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests;

public interface ISharedRequestSender
{
    //Ticket
    Task<List<TicketDto>> Send(GetAllTicketsRequestProto request);
    Task<List<TicketDto>> Send(GetTicketsForCurrentSprintRequestProto request);
    Task<List<TicketDto>> Send(GetTicketsWithShowAllSwitchRequestProto request);
    Task<TicketDto> Send(GetTicketByIdRequestProto request);
    
    //Sprint
    Task<SprintDto?> Send(GetActiveSprintRequestProto request);
    Task<List<SprintDto?>> Send(GetAllSprintsRequestProto request);
}