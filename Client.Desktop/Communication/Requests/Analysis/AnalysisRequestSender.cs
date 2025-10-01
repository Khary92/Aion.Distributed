using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.DataModels.Decorators;
using Grpc.Net.Client;
using Proto.Requests.AnalysisData;

namespace Client.Desktop.Communication.Requests.Analysis;

public class AnalysisRequestSender : IAnalysisRequestSender
{
    private readonly IAnalysisMapper _analysisMapper;
    private readonly AnalysisRequestService.AnalysisRequestServiceClient _client;

    public AnalysisRequestSender(IAnalysisMapper analysisMapper, string address)
    {
        _analysisMapper = analysisMapper;
        var channel = GrpcChannel.ForAddress(address);
        _client = new AnalysisRequestService.AnalysisRequestServiceClient(channel);
    }

    public async Task<AnalysisBySprintDecorator> Send(ClientGetSprintAnalysisById request)
    {
        var response = await _client.GetSprintAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return _analysisMapper.Create(response);
    }

    public async Task<AnalysisByTicketDecorator> Send(ClientGetTicketAnalysisById request)
    {
        var response = await _client.GetTicketAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return _analysisMapper.Create(response);
    }

    public async Task<AnalysisByTagDecorator> Send(ClientGetTagAnalysisById request)
    {
        var response = await _client.GetTagAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException();

        return _analysisMapper.Create(response);
    }
}