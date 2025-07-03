namespace Service.Monitoring.Verifiers;

public record Report(DateTimeOffset TimeStamp, Result Result, List<string> Traces);