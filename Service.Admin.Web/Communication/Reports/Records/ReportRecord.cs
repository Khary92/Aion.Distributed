namespace Service.Admin.Web.Communication.Reports.Records;

public record ReportRecord(DateTimeOffset TimeStamp, string UseCase, string State, List<ReportTrace> Traces);