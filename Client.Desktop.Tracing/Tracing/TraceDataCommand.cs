using Client.Desktop.Tracing.Tracing.Enums;

namespace Client.Desktop.Tracing.Tracing;

public record TraceDataCommand(DateTimeOffset TimeStamp, LoggingMeta LoggingMeta, Guid TraceId, string UseCaseMeta, Type OriginClassType, string Log);