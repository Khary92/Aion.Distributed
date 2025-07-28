using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels.Decorators;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.AnalysisData;

namespace Client.Desktop.Communication.Requests.Analysis;

public class AnalysisRequestSender(IAnalysisMapper analysisMapper)
    : IAnalysisRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly AnalysisRequestService.AnalysisRequestServiceClient _client = new(Channel);

    public async Task<AnalysisBySprintDecorator> Send(GetSprintAnalysisById request)
    {
        var response = await _client.GetSprintAnalysisAsync(request);

        if (response == null) throw new ArgumentNullException("Create a default implementation!");

        return analysisMapper.Create(response);
    }

    public async Task<AnalysisByTicketDecorator> Send(GetTicketAnalysisById request)
    {
        var response = await _client.GetTicketAnalysisAsync(request);

        if (response == null) throw new ArgumentNullException("Create a default implementation!");

        return analysisMapper.Create(response);
    }

    public async Task<AnalysisByTagDecorator> Send(GetTagAnalysisById request)
    {
        var response = await _client.GetTagAnalysisAsync(request);

        if (response == null) throw new ArgumentNullException("Create a default implementation!");

        return analysisMapper.Create(response);
    }
}