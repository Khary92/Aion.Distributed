using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.TicketReplay;

namespace Client.Desktop.Communication.Requests.Replays;

public class TicketReplayRequestSender(string address, ITokenService tokenService)
    : ITicketReplayRequestSender, IInitializeAsync
{
    private TicketReplayRequestService.TicketReplayRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TicketReplayRequestService.TicketReplayRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<List<DocumentationReplay>> Send(ClientGetTicketReplaysByIdRequest request)
    {
        if (_client == null)
            throw new InvalidOperationException("TicketReplayRequestSender is not initialized");

        var response = await _client.GetReplayDataAsync(request.ToProto());
        return response.ToReplayList();
    }
}