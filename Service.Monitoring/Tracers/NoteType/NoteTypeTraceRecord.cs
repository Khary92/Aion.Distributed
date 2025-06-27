using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.NoteType;

public record NoteTypeTraceRecord(
    DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TicketUseCase,
    string OriginClassType,
    string Log);