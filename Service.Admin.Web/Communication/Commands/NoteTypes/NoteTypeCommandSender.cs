using Grpc.Core;
using Grpc.Net.Client;
using Proto.Command.NoteTypes;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Commands.NoteTypes;

public class NoteTypeCommandSender : INoteTypeCommandSender
{
    private readonly JwtService _jwtService;
    private readonly NoteTypeProtoCommandService.NoteTypeProtoCommandServiceClient _client;

    public NoteTypeCommandSender(string address, JwtService jwtService)
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

        _client = new NoteTypeProtoCommandService.NoteTypeProtoCommandServiceClient(channel);
    }
    
    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task<bool> Send(CreateNoteTypeCommandProto command)
    {
        var response = await _client.CreateNoteTypeAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(ChangeNoteTypeNameCommandProto command)
    {
        var response = await _client.ChangeNoteTypeNameAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(ChangeNoteTypeColorCommandProto command)
    {
        var response = await _client.ChangeNoteTypeColorAsync(command, GetAuthHeader());
        return response.Success;
    }
}