namespace Service.Admin.Web.Communication;

public record ReportRecord(DateTimeOffset TimeStamp, string State, List<string> Traces);