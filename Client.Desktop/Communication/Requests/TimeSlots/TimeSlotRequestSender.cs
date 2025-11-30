using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.TimeSlots.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public class TimeSlotRequestSender(string address, ITokenService tokenService)
    : ITimeSlotRequestSender, IInitializeAsync
{
    private TimeSlotRequestService.TimeSlotRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TimeSlotRequestService.TimeSlotRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<TimeSlotClientModel> Send(ClientGetTimeSlotByIdRequest request)
    {
        if (_client == null) throw new InvalidOperationException("TimeSlotRequestSender is not initialized");

        var response = await _client.GetTimeSlotByIdAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return response.ToClientModel();
    }

    public async Task<List<TimeSlotClientModel>> Send(ClientGetTimeSlotsForWorkDayIdRequest request)
    {
        if (_client == null) throw new InvalidOperationException("TimeSlotRequestSender is not initialized");

        var response = await _client.GetTimeSlotsForWorkDayIdAsync(request.ToProto());
        return response == null ? [] : response.ToClientModelList();
    }
}