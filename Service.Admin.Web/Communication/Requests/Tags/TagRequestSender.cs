using Grpc.Core;
using Grpc.Net.Client;
using Proto.DTO.Tag;
using Proto.Requests.Tags;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Requests.Tags;

public class TagRequestSender : ITagRequestSender
{
    private readonly JwtService _jwtService;
    private readonly TagProtoRequestService.TagProtoRequestServiceClient _client;

    public TagRequestSender(string address, JwtService jwtService)
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
        _client = new TagProtoRequestService.TagProtoRequestServiceClient(channel);
    }

    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };
    
    public async Task<TagListProto> Send(GetAllTagsRequestProto request)
    {
        return await _client.GetAllTagsAsync(request, GetAuthHeader());
    }

    public async Task<TagProto> Send(GetTagByIdRequestProto request)
    {
        return await _client.GetTagByIdAsync(request, GetAuthHeader());
    }

    public async Task<TagListProto> Send(GetTagsByIdsRequestProto request)
    {
        return await _client.GetTagsByIdsAsync(request, GetAuthHeader());
    }
}