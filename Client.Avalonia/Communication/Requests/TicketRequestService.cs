using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.Tickets;

namespace Client.Avalonia.Communication.Requests;

public class TicketRequestSender : ITicketRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly TicketRequestService.TicketRequestServiceClient _client = new(Channel);

    public async Task<TicketListProto> GetAllTickets()
    {
        var request = new GetAllTicketsRequestProto();
        var response = await _client.GetAllTicketsAsync(request);
        return response;
    }

    public async Task<TicketListProto> GetTicketsForCurrentSprint()
    {
        var request = new GetTicketsForCurrentSprintRequestProto();
        var response = await _client.GetTicketsForCurrentSprintAsync(request);
        return response;
    }

    public async Task<TicketListProto> GetTicketsWithShowAllSwitch(bool isShowAll)
    {
        var request = new GetTicketsWithShowAllSwitchRequestProto { IsShowAll = isShowAll };
        var response = await _client.GetTicketsWithShowAllSwitchAsync(request);
        return response;
    }
}