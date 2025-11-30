using Grpc.Core;
using Grpc.Net.Client;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Requests.Tickets;

public class TicketRequestSender : ITicketRequestSender
{
    private readonly JwtService _jwtService;
    private readonly TicketProtoRequestService.TicketProtoRequestServiceClient _client;

    public TicketRequestSender(string address, JwtService jwtService)
    {
        _jwtService = jwtService;
        var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan
            },
            Credentials = ChannelCredentials.Insecure,
            UnsafeUseInsecureChannelCallCredentials = true
        });
        _client = new TicketProtoRequestService.TicketProtoRequestServiceClient(channel);
    }

    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task<TicketListProto> Send(GetAllTicketsRequestProto request)
    {
        return await _client.GetAllTicketsAsync(request, GetAuthHeader());
    }

    public async Task<TicketListProto> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        return await _client.GetTicketsForCurrentSprintAsync(request, GetAuthHeader());
    }

    public async Task<TicketListProto> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        return await _client.GetTicketsWithShowAllSwitchAsync(request, GetAuthHeader());
    }

    public async Task<TicketProto> Send(GetTicketByIdRequestProto request)
    {
        return await _client.GetTicketByIdAsync(request, GetAuthHeader());
    }
}