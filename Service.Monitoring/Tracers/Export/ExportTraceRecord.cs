using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Export;

public record ExportTraceRecord(DateTimeOffset TimeStamp,
    LoggingMeta LoggingMeta,
    string TicketUseCase,
    string OriginClassType,
    string Log);