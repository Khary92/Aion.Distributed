using Grpc.Core;
using Proto.Report;

namespace Service.Admin.Web.Communication;

public class ReportReceiver : ReportProtoService.ReportProtoServiceBase, IReportReceiver
{
    public event EventHandler<ReportRecord>? ReportReceived;

    public override Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context)
    {
        OnReportReceived(new ReportRecord(DateTimeOffset.Now, request.State, request.Traces.ToList()));
        return Task.FromResult(new ResponseProto{Success = true});
    }

    protected virtual void OnReportReceived(ReportRecord e)
    {
        ReportReceived?.Invoke(this, e);
    }
}