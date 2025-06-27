using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Ticket;

public record TicketTraceRecord(DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TicketUseCase,
    Type OriginClassType,
    string Log);