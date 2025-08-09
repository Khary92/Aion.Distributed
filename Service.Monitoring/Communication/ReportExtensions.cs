using Google.Protobuf.WellKnownTypes;
using Proto.Report;
using Service.Monitoring.Shared;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Communication;

public static class ReportExtensions
{
    public static ReportProto ToProto(this Report report)
    {
        return new ReportProto
        {
            TimeStamp = Timestamp.FromDateTimeOffset(report.TimeStamp),
            SortType = report.SortingType.ToString(),
            UseCase = report.UseCase.ToString(),
            State = report.Result.ToString(),
            Traces = { report.Traces.ToProto() }
        };
    }

    private static List<ReportTraceProto> ToProto(this List<TraceData> traces)
    {
        return traces.Select(trace => new ReportTraceProto
        {
            TimeStamp = Timestamp.FromDateTimeOffset(trace.TimeStamp),
            LoggingMeta = trace.LoggingMeta.ToString(),
            OriginClass = trace.OriginClassType.ToString(),
            Log = trace.Log
        }).ToList();
    }
}