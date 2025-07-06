using Grpc.Net.Client;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;

namespace Service.Proto.Shared.Requests.Tickets;

public class TicketRequestSender : ITicketRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress("http://127.0.0.1:8081");
    private readonly TicketRequestService.TicketRequestServiceClient _client = new(Channel);

    public async Task<TicketListProto> Send(GetAllTicketsRequestProto request)
    {
        return await _client.GetAllTicketsAsync(request);
    }

    public async Task<TicketListProto> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        return await _client.GetTicketsForCurrentSprintAsync(request);
    }

    public async Task<TicketListProto> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        return await _client.GetTicketsWithShowAllSwitchAsync(request);
    }

    public async Task<TicketProto> Send(GetTicketByIdRequestProto request)
    {
        return await _client.GetTicketByIdAsync(request);
    }
}