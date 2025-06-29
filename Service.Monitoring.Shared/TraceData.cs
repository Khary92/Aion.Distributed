using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Shared;

public record TraceData(
    TraceSinkId TraceSinkId,
    UseCaseMeta UseCaseMeta,
    LoggingMeta LoggingMeta,
    string OriginClassType,
    Guid TraceId,
    string Log,
    DateTimeOffset TimeStamp);