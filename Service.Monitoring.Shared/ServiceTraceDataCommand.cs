using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Shared;

public record ServiceTraceDataCommand(
    SortingType SortingType,
    UseCaseMeta UseCaseMeta,
    LoggingMeta LoggingMeta,
    Type OriginClassType,
    Guid TraceId,
    string Log,
    DateTimeOffset TimeStamp);