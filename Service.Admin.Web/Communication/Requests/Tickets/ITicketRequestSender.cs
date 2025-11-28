using Proto.DTO.Ticket;
using Proto.Requests.Tickets;

namespace Service.Admin.Web.Communication.Requests.Tickets;

public interface ITicketRequestSender
{
    Task<TicketListProto> Send(GetAllTicketsRequestProto request);
    Task<TicketListProto> Send(GetTicketsForCurrentSprintRequestProto request);
    Task<TicketListProto> Send(GetTicketsWithShowAllSwitchRequestProto request);
    Task<TicketProto> Send(GetTicketByIdRequestProto request);
}