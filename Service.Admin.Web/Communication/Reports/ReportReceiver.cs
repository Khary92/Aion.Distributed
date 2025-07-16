using Grpc.Core;
using Proto.Report;
using Service.Admin.Web.Communication.Reports.State;

namespace Service.Admin.Web.Communication.Reports;

public class ReportReceiver(IReportStateService reportState, ILogger<ReportReceiver> logger)
    : ReportProtoService.ReportProtoServiceBase, IReportReceiver
{
    public override Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context)
    {
        logger.LogInformation("Received report with state: {State}", request.State);
        
        var report = new ReportRecord(Guid.NewGuid(), DateTimeOffset.Now, request.State, request.Traces.ToList());
            
        reportState.AddReport(report);
        
        return Task.FromResult(new ResponseProto { Success = true });
    }
}