using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.TimerSettings;

namespace Client.Desktop.Communication.Requests.Timer;

public class TimerSettingsRequestSender(string address, ITokenService tokenService)
    : ITimerSettingsRequestSender, IInitializeAsync
{
    private TimerSettingsProtoRequestService.TimerSettingsProtoRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TimerSettingsProtoRequestService.TimerSettingsProtoRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<TimerSettingsClientModel> Send(GetTimerSettingsRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TimerSettingsRequestSender is not initialized");

        var timerSettingsAsync = await _client.GetTimerSettingsAsync(request);
        return timerSettingsAsync.ToClientModel();
    }

    public async Task<bool> Send(IsTimerSettingExistingRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TimerSettingsRequestSender is not initialized");

        var response = await _client.IsTimerSettingExistingAsync(request);
        return response.Exists;
    }
}