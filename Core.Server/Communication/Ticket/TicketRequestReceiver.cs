using Grpc.Core;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;
using Service.Server.Old.Services.Entities.Tickets;
using TicketRequestService = Proto.Requests.Tickets.TicketRequestService;

namespace Service.Server.Communication.Ticket;

public class TicketRequestReceiver(ITicketRequestsService ticketRequestsService)
    : TicketRequestService.TicketRequestServiceBase
{
    public override async Task<TicketListProto> GetAllTickets(GetAllTicketsRequestProto request,
        ServerCallContext context)
    {
        var tickets = await ticketRequestsService.GetAll();
        return tickets.ToProtoList();
    }

    public override async Task<TicketListProto> GetTicketsForCurrentSprint(
        GetTicketsForCurrentSprintRequestProto request,
        ServerCallContext context)
    {
        var tickets = await ticketRequestsService.GetTicketsForCurrentSprint();
        return tickets.ToProtoList();
    }

    public override async Task<TicketProto?> GetTicketById(GetTicketByIdRequestProto request,
        ServerCallContext context)
    {
        var ticket = await ticketRequestsService.GetTicketById(Guid.Parse(request.TicketId));
        return ticket?.ToProto();
    }

    public override async Task<TicketListProto> GetTicketsWithShowAllSwitch(GetTicketsWithShowAllSwitchRequestProto request,
        ServerCallContext context)
    {
        var tickets = request.IsShowAll
            ? await ticketRequestsService.GetAll()
            : await ticketRequestsService.GetTicketsForCurrentSprint();
        return tickets.ToProtoList();
    }
}