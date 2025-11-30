using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests.WorkDays;

public class WorkDayRequestSender(string address, ITokenService tokenService) : IWorkDayRequestSender, IInitializeAsync
{
    private WorkDayRequestService.WorkDayRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new WorkDayRequestService.WorkDayRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<List<WorkDayClientModel>> Send(ClientGetAllWorkDaysRequest request)
    {
        if (_client == null) throw new InvalidOperationException("WorkDayRequestSender is not initialized");

        var response = await _client.GetAllWorkDaysAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModelList();
    }

    public async Task<WorkDayClientModel> Send(ClientGetSelectedWorkDayRequest request)
    {
        if (_client == null) throw new InvalidOperationException("WorkDayRequestSender is not initialized");

        var response = await _client.GetSelectedWorkDayAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModel();
    }

    public async Task<WorkDayClientModel> Send(ClientGetWorkDayByDateRequest request)
    {
        if (_client == null) throw new InvalidOperationException("WorkDayRequestSender is not initialized");

        var response = await _client.GetWorkDayByDateAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModel();
    }

    public async Task<bool> Send(ClientIsWorkDayExistingRequest request)
    {
        if (_client == null) throw new InvalidOperationException("WorkDayRequestSender is not initialized");

        var response = await _client.IsWorkDayExistingAsync(request.ToProto());
        return response.Exists;
    }
}