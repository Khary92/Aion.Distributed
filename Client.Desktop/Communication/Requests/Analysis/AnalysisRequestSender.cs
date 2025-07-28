using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Analysis.Records;
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

    public async Task<AnalysisBySprintDecorator> Send(ClientGetSprintAnalysisById request)
    {
        var response = await _client.GetSprintAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException("Create a default implementation!");

        return analysisMapper.Create(response);
    }

    public async Task<AnalysisByTicketDecorator> Send(ClientGetTicketAnalysisById request)
    {
        var response = await _client.GetTicketAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException("Create a default implementation!");

        return analysisMapper.Create(response);
    }

    public async Task<AnalysisByTagDecorator> Send(ClientGetTagAnalysisById request)
    {
        var response = await _client.GetTagAnalysisAsync(request.ToProto());

        if (response == null) throw new ArgumentNullException("Create a default implementation!");

        return analysisMapper.Create(response);
    }
}