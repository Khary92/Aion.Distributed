using System;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Client.Tracing;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Command.TraceData;
using Service.Monitoring.Shared;

namespace Client.Desktop.Communication.Commands.Tracing;

public class TracingDataSender(string address, ITokenService tokenService) : ITracingDataSender, IInitializeAsync
{
    private TraceDataCommandProtoService.TraceDataCommandProtoServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TraceDataCommandProtoService.TraceDataCommandProtoServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<bool> Send(ServiceTraceDataCommand command)
    {
        if (_client is null)
            throw new InvalidOperationException("TracingDataSender is not initialized");

        var response = await _client.SendTraceDataAsync(command.ToProto());
        return response.Success;
    }
}