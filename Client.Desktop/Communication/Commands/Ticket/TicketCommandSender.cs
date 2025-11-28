using System;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Command.Tickets;

namespace Client.Desktop.Communication.Commands.Ticket;

public class TicketCommandSender(string address, ITokenService tokenService) : ITicketCommandSender, IInitializeAsync
{
    private TicketCommandProtoService.TicketCommandProtoServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TicketCommandProtoService.TicketCommandProtoServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<bool> Send(ClientUpdateTicketDocumentationCommand command)
    {
        if (_client is null)
            throw new InvalidOperationException("TicketCommandSender is not initialized");

        var response = await _client.UpdateTicketDocumentationAsync(command.ToProto());
        return response.Success;
    }
}