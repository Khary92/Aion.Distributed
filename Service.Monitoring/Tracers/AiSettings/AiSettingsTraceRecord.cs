using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.AiSettings;

public record AiSettingsTraceRecord(DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TicketUseCase,
    string OriginClassType,
    string Log);