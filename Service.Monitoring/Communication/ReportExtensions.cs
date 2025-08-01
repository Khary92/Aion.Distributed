using Google.Protobuf.WellKnownTypes;
using Proto.Report;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Communication;

public static class ReportExtensions
{
    public static ReportProto ToProto(this Report report)
    {
        return new ReportProto
        {
            TimeStamp = Timestamp.FromDateTimeOffset(report.TimeStamp),
            UseCase = report.useCase.ToString(),
            State = report.Result.ToString(),
            Traces = { report.Traces }
        };
    }
}