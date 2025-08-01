using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Enums;

namespace Service.Monitoring.Verifiers.Common.Records;

public record Report(DateTimeOffset TimeStamp, UseCaseMeta useCase, Result Result, List<string> Traces, Guid TraceId);