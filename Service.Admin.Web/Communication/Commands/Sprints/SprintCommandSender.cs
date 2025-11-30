using Grpc.Core;
using Grpc.Net.Client;
using Proto.Command.Sprints;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Commands.Sprints;

public class SprintCommandSender : ISprintCommandSender
{
    private readonly JwtService _jwtService;
    private readonly SprintCommandProtoService.SprintCommandProtoServiceClient _client;

    public SprintCommandSender(string address, JwtService jwtService)
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
        _client = new SprintCommandProtoService.SprintCommandProtoServiceClient(channel);
    }

    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task<bool> Send(CreateSprintCommandProto command)
    {
        var response = await _client.CreateSprintAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(AddTicketToActiveSprintCommandProto command)
    {
        var response = await _client.AddTicketToActiveSprintAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(SetSprintActiveStatusCommandProto command)
    {
        var response = await _client.SetSprintActiveStatusAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(UpdateSprintDataCommandProto command)
    {
        var response = await _client.UpdateSprintDataAsync(command, GetAuthHeader());
        return response.Success;
    }
}