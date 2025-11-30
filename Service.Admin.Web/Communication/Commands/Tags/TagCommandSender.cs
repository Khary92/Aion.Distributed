using Grpc.Core;
using Grpc.Net.Client;
using Proto.Command.Tags;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Commands.Tags;

public class TagCommandSender : ITagCommandSender
{
    private readonly JwtService _jwtService;
    private readonly TagCommandProtoService.TagCommandProtoServiceClient _client;

    public TagCommandSender(string address, JwtService jwtService)
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
        _client = new TagCommandProtoService.TagCommandProtoServiceClient(channel);
    }
    
    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task<bool> Send(CreateTagCommandProto command)
    {
        var response = await _client.CreateTagAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(UpdateTagCommandProto command)
    {
        var response = await _client.UpdateTagAsync(command, GetAuthHeader());
        return response.Success;
    }
}