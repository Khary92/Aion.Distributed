using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.Sprints;

namespace Client.Desktop.Communication.Requests.Sprint;

public class SprintRequestSender(string address, ITokenService tokenService) : ISprintRequestSender, IInitializeAsync
{
    private SprintProtoRequestService.SprintProtoRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new SprintProtoRequestService.SprintProtoRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<SprintClientModel?> Send(GetActiveSprintRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("SprintRequestSender is not initialized");

        var response = await _client.GetActiveSprintAsync(request);
        return response.SprintId == string.Empty ? null : response.ToClientModel();
    }

    public async Task<List<SprintClientModel>> Send(GetAllSprintsRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("SprintRequestSender is not initialized");

        var allSprintsAsync = await _client.GetAllSprintsAsync(request);
        //TODO check this warning...
        return allSprintsAsync.ToClientModelList();
    }
}