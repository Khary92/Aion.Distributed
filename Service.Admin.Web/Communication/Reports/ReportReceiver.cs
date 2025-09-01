using Grpc.Core;
using Proto.Report;
using Service.Admin.Web.Communication.Reports.Records;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Reports;

public class ReportReceiver(IReportStateServiceFactory reportStateServiceFactory)
    : ReportProtoService.ReportProtoServiceBase
{
    public override Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context)
    {
        var report = new ReportRecord(DateTimeOffset.Now, request.UseCase, request.State, request.LatencyInMs,
            request.Traces.ToReportTrace());
        
        reportStateServiceFactory.GetService(SortingType.Overview)!.AddReport(report);

        var sortingType = Enum.Parse<SortingType>(request.SortType);
        reportStateServiceFactory.GetService(sortingType)!.AddReport(report);

        return Task.FromResult(new ResponseProto { Success = true });
    }
}