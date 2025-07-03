using Grpc.Core;
using Proto.Report;

namespace Service.Admin.Web.Communication;

public interface IReportReceiver
{
    event EventHandler<ReportRecord> ReportReceived;
    Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context);
}