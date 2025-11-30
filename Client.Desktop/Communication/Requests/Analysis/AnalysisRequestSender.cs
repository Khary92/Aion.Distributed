using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.AnalysisData;

namespace Client.Desktop.Communication.Requests.Analysis;

public class AnalysisRequestSender(IAnalysisMapper analysisMapper, string address, ITokenService tokenService)
    : IAnalysisRequestSender, IInitializeAsync
{
    private AnalysisRequestService.AnalysisRequestServiceClient? _client;

    public async Task<AnalysisBySprintDecorator> Send(ClientGetSprintAnalysisById request)
    {
        var response = await _client.GetSprintAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return analysisMapper.Create(response);
    }

    public async Task<AnalysisByTicketDecorator> Send(ClientGetTicketAnalysisById request)
    {
        var response = await _client.GetTicketAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return analysisMapper.Create(response);
    }

    public async Task<AnalysisByTagDecorator> Send(ClientGetTagAnalysisById request)
    {
        var response = await _client.GetTagAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return analysisMapper.Create(response);
    }

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new AnalysisRequestService.AnalysisRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }
}