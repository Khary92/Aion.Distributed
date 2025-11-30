using Grpc.Core;
using Grpc.Net.Client;
using Proto.Command.TimerSettings;
using Service.Admin.Web.Communication.Authentication;

namespace Service.Admin.Web.Communication.Commands.TimerSettings;

public class TimerSettingsCommandSender : ITimerSettingsCommandSender
{
    private readonly JwtService _jwtService;
    private readonly TimerSettingsCommandProtoService.TimerSettingsCommandProtoServiceClient _client;

    public TimerSettingsCommandSender(string address, JwtService jwtService)
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
        _client = new TimerSettingsCommandProtoService.TimerSettingsCommandProtoServiceClient(channel);
    }
    
    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task<bool> Send(CreateTimerSettingsCommandProto command)
    {
        var response = await _client.CreateTimerSettingsAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command)
    {
        var response = await _client.ChangeSnapshotSaveIntervalAsync(command, GetAuthHeader());
        return response.Success;
    }

    public async Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command)
    {
        var response = await _client.ChangeDocuTimerSaveIntervalAsync(command, GetAuthHeader());
        return response.Success;
    }
}