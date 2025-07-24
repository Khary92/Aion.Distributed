using Core.Server.Services.Entities.Tickets;
using Grpc.Core;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;

namespace Core.Server.Communication.Endpoints.Ticket;

public class TicketRequestReceiver(ITicketRequestsService ticketRequestsService)
    : TicketProtoRequestService.TicketProtoRequestServiceBase
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

    public override async Task<TicketListProto> GetTicketsWithShowAllSwitch(
        GetTicketsWithShowAllSwitchRequestProto request,
        ServerCallContext context)
    {
        var tickets = request.IsShowAll
            ? await ticketRequestsService.GetAll()
            : await ticketRequestsService.GetTicketsForCurrentSprint();
        return tickets.ToProtoList();
    }
}