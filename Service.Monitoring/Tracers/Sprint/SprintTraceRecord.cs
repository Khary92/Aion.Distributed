using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Sprint;

public record SprintTraceRecord(
    DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TicketUseCase,
    Type OriginClassType,
    string Log);