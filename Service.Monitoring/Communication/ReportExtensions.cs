using Google.Protobuf.WellKnownTypes;
using Proto.Report;
using Service.Monitoring.Verifiers;

namespace Service.Monitoring.Communication;

public static class ReportExtensions
{
    public static ReportProto ToProto(this Report report)
        => new()
        {
            TimeStamp = Timestamp.FromDateTimeOffset(report.TimeStamp),
            State = report.Result.ToString(),
            Traces = { report.Traces }
        };
}