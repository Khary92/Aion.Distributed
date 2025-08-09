using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Web.Communication.Reports.Records;

public record ReportTrace(DateTimeOffset TimeStamp, LoggingMeta LoggingMeta, string OriginClass, string Log);