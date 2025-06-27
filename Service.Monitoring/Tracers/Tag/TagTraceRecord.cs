using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Tag;

public record TagTraceRecord(
    DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TagUseCase,
    string OriginClassType,
    string Log);