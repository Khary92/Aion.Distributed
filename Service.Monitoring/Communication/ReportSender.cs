using Grpc.Net.Client;
using Proto.Report;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Communication;

public class ReportSender : IReportSender
{
    private readonly ReportProtoService.ReportProtoServiceClient _client;

    public ReportSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new ReportProtoService.ReportProtoServiceClient(channel);
    }

    public async Task Send(Report report)
    {
        await _client.SendReportAsync(report.ToProto());
    }
}