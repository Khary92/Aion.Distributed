using Grpc.Core;
using Proto.Report;

namespace Service.Admin.Web.Communication.Reports;

public class ReportReceiver(ReportHub reportHub, ILogger<ReportReceiver> logger)
    : ReportProtoService.ReportProtoServiceBase, IReportReceiver
{
    public override async Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context)
    {
        logger.LogInformation("Received report with state: {State}", request.State);
        await reportHub.SendMessage(new ReportRecord(DateTimeOffset.Now, request.State, request.Traces.ToList()));
        return new ResponseProto { Success = true };
    }
}