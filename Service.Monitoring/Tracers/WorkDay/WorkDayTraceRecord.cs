using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.WorkDay;

public record WorkDayTraceRecord(
    DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TicketUseCase,
    Type OriginClassType,
    string Log);