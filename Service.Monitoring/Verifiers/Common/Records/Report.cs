using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Enums;

namespace Service.Monitoring.Verifiers.Common.Records;

public record Report(
    DateTimeOffset TimeStamp,
    SortingType SortingType,
    UseCaseMeta UseCase,
    Result Result,
    int LatencyInMs,
    List<TraceData> Traces,
    Guid TraceId);