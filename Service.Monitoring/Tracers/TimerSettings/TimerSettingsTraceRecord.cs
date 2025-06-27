using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.TimerSettings;

public record TimerSettingsTraceRecord(
    DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TicketUseCase,
    Type OriginClassType,
    string Log);