using Grpc.Core;
using Grpc.Net.Client;
using Proto.DTO.TimerSettings;
using Proto.Requests.TimerSettings;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Requests.TimerSettings;

public class TimerSettingsRequestSender : ITimerSettingsRequestSender
{
    private readonly JwtService _jwtService;
    private readonly TimerSettingsProtoRequestService.TimerSettingsProtoRequestServiceClient _client;

    public TimerSettingsRequestSender(string address, JwtService jwtService)
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
        _client = new TimerSettingsProtoRequestService.TimerSettingsProtoRequestServiceClient(channel);
    }

    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task<TimerSettingsProto> Send(GetTimerSettingsRequestProto request)
    {
        return await _client.GetTimerSettingsAsync(request, GetAuthHeader());
    }

    public async Task<bool> Send(IsTimerSettingExistingRequestProto request)
    {
        var response = await _client.IsTimerSettingExistingAsync(request, GetAuthHeader());
        return response.Exists;
    }
}