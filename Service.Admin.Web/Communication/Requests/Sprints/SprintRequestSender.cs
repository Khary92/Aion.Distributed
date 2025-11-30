using Grpc.Core;
using Grpc.Net.Client;
using Proto.DTO.Sprint;
using Proto.Requests.Sprints;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Requests.Sprints;

public class SprintRequestSender : ISprintRequestSender
{
    private readonly JwtService _jwtService;
    private readonly SprintProtoRequestService.SprintProtoRequestServiceClient _client;

    public SprintRequestSender(string address, JwtService jwtService)
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
        _client = new SprintProtoRequestService.SprintProtoRequestServiceClient(channel);
    }

    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };
    
    public async Task<SprintProto?> Send(GetActiveSprintRequestProto request)
    {
        var response = await _client.GetActiveSprintAsync(request, GetAuthHeader());
        return response.SprintId == string.Empty ? null : response;
    }

    public async Task<SprintListProto> Send(GetAllSprintsRequestProto request)
    {
        return await _client.GetAllSprintsAsync(request, GetAuthHeader());
    }
}