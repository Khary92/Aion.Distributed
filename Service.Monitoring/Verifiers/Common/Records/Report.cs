using Service.Monitoring.Verifiers.Common.Enums;

namespace Service.Monitoring.Verifiers.Common.Records;

public record Report(DateTimeOffset TimeStamp, Result Result, List<string> Traces);