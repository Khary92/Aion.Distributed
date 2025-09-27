namespace Service.Admin.Web.Communication.Records;

public record ReportRecord(
    DateTimeOffset TimeStamp,
    string UseCase,
    string State,
    int LatencyInMs,
    List<ReportTrace> Traces);