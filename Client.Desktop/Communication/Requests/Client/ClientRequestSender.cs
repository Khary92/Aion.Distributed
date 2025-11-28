using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.Client;

namespace Client.Desktop.Communication.Requests.Client;

public class ClientRequestSender(string address, ITokenService tokenService) : IClientRequestSender, IInitializeAsync
{
    private ClientRequestService.ClientRequestServiceClient? _client;

    public async Task<List<ClientGetTrackingControlResponse>> Send(ClientGetTrackingControlDataRequest request)
    {
        if (_client is null)
            throw new InvalidOperationException("Client is not initialized");

        var timeSlotControlDatalist = await _client.GetTrackingControlDataByDateAsync(request.ToProto());
        return timeSlotControlDatalist.ToResponseDataList();
    }

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new ClientRequestService.ClientRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }
}