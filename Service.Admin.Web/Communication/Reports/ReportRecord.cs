namespace Service.Admin.Web.Communication.Reports;

public record ReportRecord(DateTimeOffset TimeStamp, string State, List<string> Traces);