using Grpc.Core;
using Proto.Report;

namespace Service.Admin.Web.Communication.Reports;

public interface IReportReceiver
{
    Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context);
}