using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Client.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Command.Client;

namespace Client.Desktop.Communication.Commands.Client;

public class ClientCommandSender(string address, ITokenService tokenService) : IClientCommandSender, IInitializeAsync
{
    private ClientCommandProtoService.ClientCommandProtoServiceClient? _client;

    public async Task<bool> Send(ClientCreateTrackingControlCommand command)
    {
        if (_client is null) throw new InvalidOperationException("ClientCommandSender is not initialized");

        var response = await _client.CreateTimeSlotControlAsync(command.ToProto());
        return response.Success;
    }

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new ClientCommandProtoService.ClientCommandProtoServiceClient(callInvoker);

        await Task.CompletedTask;
    }
}