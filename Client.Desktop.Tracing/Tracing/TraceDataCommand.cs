using Client.Desktop.Proto.Tracing.Enums;

namespace Client.Desktop.Tracing.Tracing;

public record TraceDataCommand(
    TraceSinkId TraceSinkId,
    UseCaseMeta UseCaseMeta,
    LoggingMeta LoggingMeta,
    Type OriginClassType,
    Guid TraceId,
    string Log,
    DateTimeOffset TimeStamp);