namespace Service.Admin.Web.Communication.Reports;

public record ReportRecord(Guid ReportId, DateTimeOffset TimeStamp, string State, List<string> Traces);