using Google.Protobuf.Collections;
using Proto.Report;
using Service.Admin.Web.Communication.Reports.Records;
using Service.Monitoring.Shared.Enums;
using Enum = System.Enum;

namespace Service.Admin.Web.Communication.Reports;

public static class ReportExtensions
{
    public static ReportRecord ToReportRecord(this ReportProto report) => new(
        report.TimeStamp.ToDateTimeOffset(), report.UseCase, report.State, report.LatencyInMs,
        report.Traces.ToReportTrace());


    public static List<ReportTrace> ToReportTrace(this RepeatedField<ReportTraceProto> reportTraceProtos)
    {
        return reportTraceProtos
            .Select(reportTraceProto => new ReportTrace(reportTraceProto.TimeStamp.ToDateTimeOffset(),
                Enum.Parse<LoggingMeta>(reportTraceProto.LoggingMeta), reportTraceProto.OriginClass,
                reportTraceProto.Log)).ToList();
    }
}