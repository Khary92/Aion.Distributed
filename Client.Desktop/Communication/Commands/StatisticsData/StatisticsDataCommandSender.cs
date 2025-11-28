using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Command.StatisticsData;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public class StatisticsDataCommandSender(string address, ITokenService tokenService)
    : IStatisticsDataCommandSender, IInitializeAsync
{
    private StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<bool> Send(ClientChangeTagSelectionCommand command)
    {
        if (_client is null)
            throw new InvalidOperationException("StatisticsDataCommandSender is not initialized");

        var response = await _client.ChangeTagSelectionAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientChangeProductivityCommand command)
    {
        if (_client is null)
            throw new InvalidOperationException("StatisticsDataCommandSender is not initialized");

        var response = await _client.ChangeProductivityAsync(command.ToProto());
        return response.Success;
    }
}