using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.StatisticsData.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.StatisticsData;

namespace Client.Desktop.Communication.Requests.StatisticsData;

public class StatisticsDataRequestSender(string address, ITokenService tokenService)
    : IStatisticsDataRequestSender, IInitializeAsync
{
    private StatisticsDataRequestService.StatisticsDataRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new StatisticsDataRequestService.StatisticsDataRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<StatisticsDataClientModel> Send(ClientGetStatisticsDataByTimeSlotIdRequest request)
    {
        if (_client is null) throw new InvalidOperationException("StatisticsDataRequestSender is not initialized");

        var response = await _client.GetStatisticsDataByTimeSlotIdAsync(request.ToProto());
        return response.ToClientModel();
    }
}