using Grpc.Core;
using Proto.Report;

namespace Service.Admin.Web.Communication;

public class ReportReceiver(IReportEventHandler reportEventHandler, ILogger<ReportReceiver> logger) : ReportProtoService.ReportProtoServiceBase, IReportReceiver
{
    public override Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context)
    {
        logger.LogInformation("Received report with state: {State}", request.State);
        reportEventHandler.OnReportReceived(new ReportRecord(DateTimeOffset.Now, request.State, request.Traces.ToList()));
        return Task.FromResult(new ResponseProto{Success = true});
    }
}