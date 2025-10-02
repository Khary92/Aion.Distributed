using Grpc.Core;
using Proto.Report;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Communication.Records;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Receiver.Reports;

public class ReportReceiver(IReportStateServiceFactory reportStateServiceFactory)
    : ReportProtoService.ReportProtoServiceBase
{
    public override Task<ResponseProto> SendReport(ReportProto request, ServerCallContext context)
    {
        var report = new ReportRecord(DateTimeOffset.Now, request.UseCase, request.State, request.LatencyInMs,
            request.Traces.ToReportTrace());

        var overviewStateService = reportStateServiceFactory.GetService(SortingType.Overview);

        if (overviewStateService == null) throw new Exception("Overview ReportStateService not found");

        overviewStateService.AddReport(report);

        var sortingType = Enum.Parse<SortingType>(request.SortType);
        var reportStateService = reportStateServiceFactory.GetService(sortingType);

        if (reportStateService == null)
            throw new Exception("ReportStateService for " + request.SortType + " not found");

        reportStateService.AddReport(report);

        return Task.FromResult(new ResponseProto { Success = true });
    }
}