using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Note;

public record NoteTraceRecord(DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    Guid TraceId,
    string TicketUseCase,
    string OriginClassType,
    string Log);