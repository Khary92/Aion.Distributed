using Grpc.Core;
using Grpc.Net.Client;
using Proto.Command.Tickets;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Commands.Tickets;

public class TicketCommandSender : ITicketCommandSender
{
    private readonly JwtService _jwtService;
    private readonly TicketCommandProtoService.TicketCommandProtoServiceClient _client;

    public TicketCommandSender(string address, JwtService jwtService)
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
        _client = new TicketCommandProtoService.TicketCommandProtoServiceClient(channel);
    }
    
    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };


    public async Task<bool> Send(CreateTicketCommandProto command)
    {
        var response = await _client.CreateTicketAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDataCommandProto command)
    {
        var response = await _client.UpdateTicketDataAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommandProto command)
    {
        var response = await _client.UpdateTicketDocumentationAsync(command, GetAuthHeader());
        return response.Success;
    }
}