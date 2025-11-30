using Grpc.Core;
using Grpc.Net.Client;
using Proto.Report;
using Service.Monitoring.Communication.Authentication;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Communication;

public class ReportSender : IReportSender
{
    private readonly JwtService _jwtService;
    private readonly ReportProtoService.ReportProtoServiceClient _client;

    public ReportSender(string address, JwtService jwtService)
    {
        _jwtService = jwtService;
        var channel = GrpcChannel.ForAddress(address);
        _client = new ReportProtoService.ReportProtoServiceClient(channel);
    }
    
    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task Send(Report report)
    {
        await _client.SendReportAsync(report.ToProto());
    }
}